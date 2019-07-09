## Experimental Product Domain Based On Hexagonal Architecture Principles
This project is a sample application built using .NET Core. The main goal of this project is implementing and better understanding DDD and hexagonal architecture principles.  
# Also known as
* Ports and Adapters
* Clean Architecture
* Onion Architecture
# Hexagonal architecture
![hexagonal](/hexagonal.png?raw=true "hexagonal")



With hexagonal architecture
* Domain contains enterprise wide logic and types and does not depend anything except these 
* Application contains business logic and types
* Infrastructure (including persistence, messaging, logging, presentation) contains all external concerns
* Presentation and Infrastructure depend only on Application
* Infrastructure dependencies can be replaced
with minimal effort. For instance, we can switch data store without touching business code. 

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio Code or 2017](https://www.visualstudio.com/downloads/)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory, restore dependencies
     ```
     dotnet restore
     ```
  3. Build the solution
     ```
     dotnet build
     ```
  5. Run tests
     ```
	    dotnet test
	 ```
## Technologies
* .NET Core 2.2
* ASP.NET Core 2.2
## License
This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/CanerPatir/aspnet-core-clean-arch/blob/master/LICENSE) file for details.
## References
* https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/
* https://blog.ploeh.dk/2013/12/03/layers-onions-ports-adapters-its-all-the-same/