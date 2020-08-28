# Lightest Night
## Data > MySql

MySql Data Components

### Build Status
![](https://github.com/lightest-night/system.data.mysql/workflows/CI/badge.svg)
![](https://github.com/lightest-night/system.data.mysql/workflows/Release/badge.svg)

#### How To Use
##### Registration
* Asp.Net Standard/Core Dependency Injection
  * Use the provided `services.AddMySqlData(Action<MySqlOptions> options = null)` method

##### Usage
* `MySqlConnection Build()`
  * A function to call to generate a new, ready to use, but closed, instance of `MySqlConnection`