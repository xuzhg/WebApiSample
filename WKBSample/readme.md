# Well-Known Binary (WKB)

It's GUI App (WIN Form) to play Well-Known Binary and supports:
* Standard WKB
* Extended WKB
* ISO WKB

For details about WKB, please refer to: https://libgeos.org/specifications/wkb/#iso-wkb

## How to play
If you run the app, it opens a windows like below:

![image](https://github.com/user-attachments/assets/fb298c43-5e54-435d-ab29-8121511ac6fd)

What you can do is to right click on the Right List box to add Spatial types:

![image](https://github.com/user-attachments/assets/de242d4b-0f7f-4f1f-8449-72add6b2e510)

For example, you right click and select 'Add Collection', it turns out as:

![image](https://github.com/user-attachments/assets/536fc170-ae7b-44da-8a7d-28703780a1a1)

Once you add a spatial type, you can select the added spatial type and right click on it to add more sub (corresponding) spatial types on it.

![image](https://github.com/user-attachments/assets/059a611f-99f4-4ebd-a6bb-75c25e72d096)


If you want to restart it, you can click the 'Clear All' button to restart.

## How to config

You can switch the settings, for example, if you uncheck the 'SRID', 'Z' and 'M', you can get the 'Standard WKB'.

![image](https://github.com/user-attachments/assets/040c0b39-dc40-4111-953c-1c0b5e19c4ce)


## An example

There's an example at https://libgeos.org/specifications/wkb/#iso-wkb

The following bytes (in hex) make up the WKB for a LINESTRING(0 0, 1 1, 2 1):

```txt
01                - byteOrder(wkbNDR)
02000000          - wkbType(LineString)
03000000          - numPoints(3)
0000000000000000  - x(0.0)
0000000000000000  - y(0.0)
000000000000F03F  - x(1.0)
000000000000F03F  - y(1.0)
0000000000000040  - x(2.0)
000000000000F03F  - y(1.0)
```

You can verify it using the App as below:

![image](https://github.com/user-attachments/assets/87c8b78c-fbaf-46c9-8230-c843e2536ae2)

The aim/goal of this APP is to simplify the testing when supports WKB in OData.Spatial lib since this lib aleady supported WKT (Well Known Text).
