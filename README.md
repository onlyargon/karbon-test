# F1DriverApi
---

## About the 3rd party API
I have used openf1 api for this test. https://openf1.org/docs/
This is provide near real time data about formula f1. I used /driver endpoint

### No API key needed

## Config and Run
- Run the sql script in the database directory to create Drivers table
- Run ```dotnet run```
- DB hosted on azure sql server because I'm doing this on mac and I wans't able to get sql to work on docker correctly
- I have setup scalar, so when the app is running api document available on '/scalar'

