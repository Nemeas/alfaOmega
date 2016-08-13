# The idea
I am allways forgetting what the speedlimit is when I'm driving, so I just try to match the other cars speed. So what I want to make is an application to show the user what the speed-limit is on the road he is driving on and alerting him if he's driving too fast.

## Resources

* NVDB Api: https://www.vegvesen.no/nvdb/apidokumentasjon/

## Restrictions
The NVDB only holds data for Norwegian roads, so this application will only work in Norway.

## How to get this running for yourself
1. If you don't have Xamarin installed, download it here: https://www.xamarin.com/download. I believe that this will also install the Android SDK Manager and AVD, if not then it's propably best to just download it with the Android Studio: https://developer.android.com/studio/index.html.

2. Remember to add the path to android sdk to the Android_home environment variable: https://spring.io/guides/gs/android/

3. Clone or download the repo from https://github.com/Nemeas/alfaOmega.

4. Open the .sln file.

And you should be good to go.


## Status
>13.08.2016- It looks like I'm getting the correct data based on setting the gps on my android emulator to different spots around where I live where I know what speedlimit it is. Need to take it out for a spin to see if it updates correctly.

>12.08.2016 - Currently working on a poc to see if this is even possible. Having a question going on Stackoverflow: http://stackoverflow.com/questions/38906684/api-request-fails-in-application-but-works-in-browser

## The way ahead

1. Display the users speed next to the speedlimit and set a red background for when the user is driving faster than the speedlimit.
2. Not only set the background red, but also display how much it will cost to be caught driving over the speed limit.
3. Add other signs like (are you driving on a priority road or do you have to yield for other cars? elk-sign? etc.).

_____________________
_This README.md was made with the help of https://jbt.github.io/markdown-editor._