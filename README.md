# Food Delivery Shipping Service

## The project
This repository is the code base of Shipping service - a part of my Food Delivery Application system that applies GeoHash into Shipper searching business.
My main goal on the project so far is to create a proof-of-concept system design for geo-proximity searching based on GeoHash library. Therefore, I just simplified
other things, e.g: shipper management, order management,... Maybe I will extend those features if I find things worth to try.


## The geo-searching problem
Nowadays, the need for searching nearby object has seen an enormous rise, especially in recent years with the prevalence of food delivery apps, social apps, booking apps 
and etc. It poses a challenge of delivering solution that is efficient in both time and space, meaning that we demand a search feature to be able to find the most outcomes in the least period. The challenge is more difficult for big system because the scalability and availability are always the most vital criteria.

## Geohash
Geohash is a geocode system invented in 2008 by Gustavo Niemeyer which encodes a geographic location into a short string of letters and digits.\
Geohash's idea is splitting The Earth into multiple rectangles and assigning each with a representative code. Starting with level 1, The Earth is divided into 32 squares (that aligns 4 rows and 8 columns) of 5.000km x 5.000km and each square is represented by 1 character. These squares are level-one areas. Similarly, each level-one square is divided 32 rectangles, each of which is assigned with 2-character-long code. The process happens recursively and the rectangles keep getting smaller by level.\
***E.g:** level-5 squares are  4.89km x 4.89km.*

Interestingly, the number of character in an area's code is also its level and the code is its parent area's code plus 1 character.\
***E.g:** area having code **gkpdue** is a level-6 area and is child area of **gkpdu**.*

The power this geocode system offers is that given a lat-lon coordinate, we can get the area it belongs to, at any level. Therefore, the approach for designing geo-indexing system is to arrange points on Earth into Geohash areas. When a proximity search is ordered, we can minimize the search scope by identifying the Geohash code of given location.

*You can refer to Geohash on [wiki](https://en.wikipedia.org/wiki/Geohash) page and [this site](https://www.movable-type.co.uk/scripts/geohash.html) for visualization and some explanation.*

## The problem
The problem I'm solving is to match shipper with customer's order in real time. My assumption on the scenario is as following:\
**When customer places an order, they need to specify the location of the food establishment. The system will automatically scan nearby for shippers.**

*Note: even though there are several more criteria of shipper searching in real-world systems, my assumption is sufficient for this experiment.*

Besides, shippers' location needs to be tracked in real-time.
## My system design
The main idea of my solution is to distribute shippers' locations into small Geohash areas and they need to be retrieved and updated very fast. I opted to Redis as the storage of shippers' location for following reasons:
- Redis is fast at retrieving and updating because it uses memory.
- Redis stores data at key-value structure, so by using Geohash code as key and store all shipper's data as value, I can search for shippers around an area easily.
- Redis is scalable.

I picked Microsoft SQL Server as persistent storage for the sake of familiarity. PostgreSQL is well-known for providing lots of support for geo-data storing and indexing but currently my assumed problem has not required it just yet. Maybe I'll consider it for future use-cases (that I haven't come up with).

I divided my systems into following components:
- Identity Service: manages users and auth. It is setup with Identity server 4
- Food Establishment & Ordering Service: lists food establishments and creates orders. If you would like to check its repo, it is [here](https://github.com/Minh8181-blz/Food_Delivery-order-service)
- Shipping Service: manages shipper location, matches order with available shippers (the current repo)
- Nginx gateway
- Kafka as message broker (higly scalable, suitable for expanding system)
- Redis for storing shippers' location

![alt text](https://github.com/Minh8181-blz/Food_Delivery-shipping-service/blob/master/system-diagram.png)

## Implementation
### Shipper location real-time tracking
Shipper keeps sending their current location from device to service in a certain inverval. I used 2 keys on Redis to store this data:
- **shipper_location_{shipperId}**: contains JSON string of their current lat-lon
- **area_shippers_{areaCode}**: stores a SET of shipper id who are currently present in the area having code {areaCode}

*The areaCode is fixed to **level 6** (each area is 1.22km x 0.61km wide)*

Upon receiving new shipper location, my service re-hash the location to code, then update **shipper_location_{shipperId}** value.\
If area change is detected, shipper id in *area_shippers_{oldAreaCode}* will be removed then be added to **area_shippers_{newAreaCode}**.

*As the **area_shippers_{areaCode}** key maintains a SET, getting or adding or removing an element in set only takes O(1) of complexity*

### Order-Shipper matching
Shipping service receives order events through Kafka. When event is received, shipping service pushed a **shipper-searching command** to Kafka with the search area is the one around the food store's location. The command is processed by shipping service ifself.

### Redis SCAN
I used the Redis SCAN command on **area_shippers_{areaCode}** key to get shipper ids in given area. SCAN command only fetches some elements in set and its time complexity is O(1) for every call. Hence, this operation is optimal here because we only need to find the first element that matches and we can avoid scanning the entire set. SCAN can be referred [here](https://redis.io/commands/scan/).

If an id is fetched, shipper location is then get from **shipper_location_{shipperId}** key for extra assesment (e.g: distance calculation) prior to be officially matched to order. Otherwise, the service sends another **shipper-searching command** to Kafka but with a different area that is a neighbor of previous one.

### Geohash area neighbor
Each area in Geohash has 8 neighbor areas. Given 1 area code, Geohash can quickly detect all of its 8 neighbors. I utilized this feature for expanding search area.\
In details, if current area is original (where the food establishment at), I will seek for shipper in its North neighbor. Then I just keep going clockwise around the original area until I find a shipper or I hit its North-West neighbor. From this point, the search area is reset to the original and the loop repeats.
