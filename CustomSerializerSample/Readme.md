
====================
Customize feed and entry serializer to skip the null object
====================

<h3>0</h3>

In this sample, I create an in-memory customer-order model.

```txt
Customers
   |- Customer #1
   |- Customer #2
   |- Customer #3
   |- Customer #4
   |- Customer #5
   |- null
   |- Customer #9
          |- Order #116
		  |- null
		  |- Order #117
```
Where, the sixth customer is null, and the second order in customer #9 is null.
   
<h3>1</h3>

if you send request for:
```
GET http://localhost:5948/odata/Customers
```

You can get 

```json
{
  "@odata.context":"http://localhost:5948/odata/$metadata#Customers","value":[
    {
      "Id":1,"Name":"Name#1"
    },{
      "Id":2,"Name":"Name#2"
    },{
      "Id":3,"Name":"Name#3"
    },{
      "Id":4,"Name":"Name#4"
    },{
      "Id":5,"Name":"Name#5"
    },{
      "Id":9,"Name":"Sam"
    }
  ]
}
```
There's no *null* customer.

<h3>2</h3>
if you send request for:
```
GET http://localhost:5948/odata/Customers(9)?$expand=Orders
```

You can get 

```json
{
  "@odata.context":"http://localhost:5948/odata/$metadata#Customers/$entity","Id":9,"Name":"Sam","Orders":[
    {
      "Id":116
    },{
      "Id":117
    }
  ]
}
```
There's no *null* order.
