# How to test
1. Set the project version to some value. (1.0.0.0)
2. Run publish.bat.
3. Compress the contents of the publish folder to a zip named "Simple-1.0.0.0.zip".
4. Place that zip in the update-source folder.
5. Decrement the version to some value less than step 1.
6. Run publish.bat
7. Run Simple.exe in the publish folder.