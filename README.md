# Food_Delivery-shipping-service

### The project
This repository is the code base of Shipping service - a part of my Food Delivery Application system that applies GeoHash into Shipper searching business.
My main goal on the project so far is to create a proof-of-concept system design for geo-proximity searching based on GeoHash library. Therefore, I just simplified
other things, e.g: shipper management, order management,... Maybe I will extend those features if I find things worth to try.


### The geo-searching problem
Nowadays, the need for searching nearby object has seen an enormous rise, especially in recent years with the prevalence of food delivery apps, social apps, booking apps 
and etc. It poses a challenge of delivering solution that is efficient in both time and space, meaning that we demand a search feature to be able to find the most outcomes in the least period. The challenge is more difficult for big system because the scalability and availability are always the most vital criteria.

### Geohash
Geohash is a geocode system invented in 2008 by Gustavo Niemeyer which encodes a geographic location into a short string of letters and digits.\
Geohash's idea is splitting The Earth into multiple rectangles and assigning each with a representative code. Starting with level 1, The Earth is divided into 32 squares (that aligns 4 rows and 8 columns) of 5.000km x 5.000km and each square is represented by 1 character. These squares are level-one areas. Similarly, each level-one square is divided 32 rectangles, each of which is assigned with 2-character-long code. The process happens recursively and the rectangles keep getting smaller by level.\
***E.g:** level-5 squares are  4.89km x 4.89km.*

Interestingly, the number of character in an area's code is also its level and the code is its parent area's code plus 1 character.\
***E.g:** area having code **gkpdue** is a level-6 area and is child area of **gkpdu**.*

The power this geocode system offers is that given a lat-lon coordinate, we can get the area it belongs to, at any level. Therefore, the approach for designing geo-indexing system is to arrange points on Earth into Geohash areas. When a proximity search is ordered, we can minimize the search scope by identifying the Geohash code of given location.

### The problem
The problem I'm solving is to match shipper with customer's order in real time. My assumption on the scenario is as following:\
**When customer places an order, they need to specify the location of the food establishment. The system will automatically scan nearby for shippers.**\
*Note: even though there are several more criteria of shipper searching in real-world systems, my assumption is sufficient for this experiment.*

Besides, shippers' location needs to be tracked in real-time.
### My system design
The main idea of my solution is to distribute shippers' locations into small Geohash areas and they need to be retrieved and updated very fast. I opt to Redis as the storage of shippers' location for following reasons:
- Redis is fast at retrieving and updating because it uses memory.
- Redis stores data at key-value structure, so by using Geohash code as key and store all shipper's data as value, I can search for shippers around an area easily.
- Redis is scalable.

*Updating...*
![alt text](https://github.com/Minh8181-blz/Food_Delivery-shipping-service/blob/master/system-diagram.png)
