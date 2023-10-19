[Resa Call Control System](/Home-Page/Call-Control-System.md)
=======
<!-- Chapter 2 -->
[Sub-systems and Components](/Home-Page/Call-Control-System/Subsystems-and-Components.md)
-------------------------
<!-- Section 2.2 -->
__Event Aggregator - Overview__
------

This Component is aimed to receive and deliver Events between Sub-systems.

Responsibilities
------

* Receiving Events
* Routing Events (Subscribe Receivers)
* Deliver Events
* It should be able to handle events witch have a payload data


Implementation
------


Implementation of __Resa call control system__'s Event aggregator, includes _(1)_ Setting up Event aggregation server and _(2)_ [a class library project](/Home-Page/Call-Control-System/Projects.md#infrastructure) containing __Event__ class and its related data models, an EventAggregator class as a wrapper on RabbitMQ for system components to easily interact with server and abstraction for event subscribers.

__RabbitMQ__:

Resa Call control system, uses RabbitMQ for event aggregation. 
RabbitMQ is setup and configured on its own server. Following table lists details of server configurations:

|Table 2.2.1: RabbitMQ Configuration details|
|:------------------------------------------------------:|

|Title|Description|
|:------------------|:-----------------------------------|
|Server:||
<!-- TODO: Complete this table regarding applied configurations -->

__Infrastructure__:

Infrastructure is a .Net core class library application, containing RabbitMQ wrapper and interfaces and Event class and its data models. These class set follows following class diagram.


| Figure 2.2.1: Event Aggregator Class diagram|
|:-------------------------------------------:|
|![Class Diagram](/.attachments/Call-Control-System/event-aggregator-class-diagram.png)     |

---

|Table 2.2.2: Introduced Types|
|:------------------------------------------------------:|

| Class/Interface | Descriptions |
|:---|:---|
|```Event<T>```|* Represents an event object.<br> * T: is the type of contained data model.|
|```IEventReceiver```| Each object witch receives events, must implement this interface. So, it can be registered on EventAggregator and receive its events whenever this event(s) raised somewhere in the system.|
|```IEventAggregator```| The class implementing this interface has to: <br>* Register an IEventReceiver <br>* Route an incoming event to its correct receiver(s)<br>* Be able to send an Event<br>|
| ```EventAggregator```| This is the implementation for ```IEventAggregator```. In class diagram, this class is named generally to keep the design separated from implementation, but in __Resa Call Control System__, the class:  ```RabbitMQEventAggregator``` will implement the ```IEventAggregator``` interface. This class wraps the RabbitMQ Objects and provides functionalities described for ```IEventAggregator```|


______________________________
<sub>This document has incomplete information dependant to actual implementation which is marked with TODO tag in source file.</sup>