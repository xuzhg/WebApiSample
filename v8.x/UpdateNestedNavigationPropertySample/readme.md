By default, the nested navigation property is not updatable.

# 1 using extension

We can change the deserializer to allow it.

Send the following request, you can get the correct repsonse.

<img width="935" alt="image" src="https://user-images.githubusercontent.com/9426627/197288696-de937b68-5f67-47b0-88a9-1688e5d9c7f2.png">


# 2 without extension, using delta payload

It also supports to delta payload. It means for patch to the entity set.
In this case, there's no need the extensions.

See the following example:

<img width="638" alt="image" src="https://user-images.githubusercontent.com/9426627/197290162-8a2a6dae-20ce-4361-80a7-c6e143a4b953.png">
