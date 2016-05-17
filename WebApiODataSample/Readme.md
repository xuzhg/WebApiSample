

<h3>Deep insert</h3>
===

* Deep insert is not supported yet.

====================

<h3>$ref test</h3>
===

To create ref (link) between entities, we can use **POST** or **PUT** to the $ref request.
For example:
`
~/Customers(1)/Order/$ref
`

The request payload should contain the single **@odata.id** or a collection. for example

```json
{"@odata.id":"http://localhost/odata/Orders(2)"}
```

<h4>NOTE</h4>

The link payload should be one of the below content:

1.Absolute URIs, or

2.relative URIs, but **@odata.context** annotation must be on the payload.


So, the below payload won't work. 

```json
{"@odata.id":"Orders(2)"}  
```

It should be include the **@odata.context**, for example:

```json
{"@odata.context":"http://localhost/odata/$metadata","@odata.id":"Orders(2)"}  
```
