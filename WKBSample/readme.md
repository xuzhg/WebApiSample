# Well-Known Binary (WKB)

It's GUI App (WIN Form) to play Well-Known Binary and supports:
* Standard WKB
* Extended WKB
* ISO WKB

For details about WKB, please refer to: https://libgeos.org/specifications/wkb/#iso-wkb

## How to play
If you run the app, it opens a windows like below:

![image](https://github.com/user-attachments/assets/b5445501-76ce-44e7-b957-adfb8045dd3e)

What you can do is to right click on the Right List box to add Spatial types:

![image](https://github.com/user-attachments/assets/b1df58b2-e33d-4c7a-9a42-0c99b757898e)

For example, you right click and select 'Add Collection', it turns out as:

![image](https://github.com/user-attachments/assets/02bb8261-5af1-4872-9ea1-2d31c73e0b9c)

Once you add a spatial type, you can select the added spatial type and right click on it to add more sub (corresponding) spatial types on it.

![image](https://github.com/user-attachments/assets/5a79c35c-f6a0-4691-a49b-c38fc49e31bd)

If you want to restart it, you can click the 'Reset All' button to restart.

## How to config

You can switch the settings, for example, if you uncheck the 'SRID', 'Z' and 'M', you can get the 'Standard WKB'.

![image](https://github.com/user-attachments/assets/be45d0bb-8c0c-4652-83f5-a6247190302c)


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

![image](https://github.com/user-attachments/assets/d09eef0d-b988-4073-b7e5-18a1a9772353)


