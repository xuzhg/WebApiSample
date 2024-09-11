It's related to issue: https://github.com/OData/AspNetCoreOData/issues/1310

### Get Empty string

`http://localhost:5285/empty/customers?$compute=concat(VatNumber/CountryCode,VatNumber/Number) as FullVAT&$select=FullVAT`

You will get:

![image](https://github.com/user-attachments/assets/2b23f543-d84e-4fc8-816a-92d2447d14cc)

Meanwhile, the Database outputs:

![image](https://github.com/user-attachments/assets/10e5c724-73c5-4882-b306-b6b56f8127c4)


### Get null

`http://localhost:5285/null/customers?$compute=concat(VatNumber/CountryCode,VatNumber/Number) as FullVAT&$select=FullVAT`

You will get:

![image](https://github.com/user-attachments/assets/d8e3c927-5a70-47e0-80a4-d62bb931514b)

Meanwhile, the Database outputs:

![image](https://github.com/user-attachments/assets/4d4a5c52-6bd3-42c5-86d9-8b199937597e)
