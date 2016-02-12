#!/bin/bash

mkdir -p ./tmp

# For System.Drawing/OpenTK/Xamarin.Mac on both XM 45 and Mobile, dump all public symbols, removing spam and empty lines
# sed '1,7d' removes the first 7 lines
# sed -e :a -e '$d;N;2,2ba' -e 'P;D' removes the last 2
monop -d -r:../bin/mac/mobile/System.Drawing.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/mobile_SD.txt
monop -d -r:/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/Xamarin.Mac/OpenTK.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/mobile_TK.txt
monop -d -r:/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/Xamarin.Mac/Xamarin.Mac.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/mobile_XM.txt
monop -d -r:../bin/mac/xm45/System.Drawing.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/45_SD.txt
monop -d -r:/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/4.5/OpenTK.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/45_TK.txt
monop -d -r:/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/4.5/Xamarin.Mac.dll | sed '1,7d' | sed -e :a -e '$d;N;2,2ba' -e 'P;D' > ./tmp/45_XM.txt

echo "The next four numbers should all be zero:"
# Compare the SD dump to the other existing assemblies, we should not have any lines in common
comm -12 ./tmp/mobile_SD.txt ./tmp/mobile_TK.txt | wc -l | tr -d "[:blank:]" 
comm -12 ./tmp/mobile_SD.txt ./tmp/mobile_XM.txt  | wc -l | tr -d "[:blank:]" 
comm -12 ./tmp/45_SD.txt ./tmp/45_TK.txt | wc -l | tr -d "[:blank:]" 
comm -12 ./tmp/45_SD.txt ./tmp/45_XM.txt | wc -l | tr -d "[:blank:]" 

rm -r ./tmp
