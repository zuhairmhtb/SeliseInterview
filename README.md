# Tasks completed for the interview at Selise on 6th November, 2024

## 1.0 Email attachment related to the task requirements

Programming Problem List: 

https://leetcode.com/problems/rotate-array/description/ 

SQL Problem List: 

https://leetcode.com/problems/rising-temperature/description/ 

https://leetcode.com/problems/employee-bonus/description/ 


Please Submit the assessment by 4:30 pm

Best Regards,

Fariza Tasnim Raida, MSc Global Human Resource Management 

Kingston University London 

Senior HR Generalist

Mobile: +880 1729 25 0076


## 2.0 Overview of the tasks

A technical interview was conducted at Selise on 6th November, 2024. The interview was for the position of .Net developer. The interview contained the following tasks:

### 2.1 [Rotate Array](https://leetcode.com/problems/rotate-array/description/)

**Problem**

Given an integer array nums, rotate the array to the right by k steps, where k is non-negative.


**Programming Language used: Python**


### 2.2 [Rising Temperature](https://leetcode.com/problems/rising-temperature/description/)

Write a solution to find all dates' id with higher temperatures compared to its previous dates (yesterday).

Return the result table in any order.

**Programming Language used: SQL**

### 2.3 [Employee Bonus](https://leetcode.com/problems/employee-bonus/description/ )

Write a solution to report the name and bonus amount of each employee with a bonus less than 1000.

Return the result table in any order.

The result format is in the following example.

**Programming Language used: SQL**


### 2.4 Design an Order and Product Management System (.Net)

Note: The file (Back- End Assessment.pdf)  is available in this repository.

**Objective:** The goal is to design and implement a system that manages products and
orders using CQRS and event-driven architecture. The system should be loosely coupled
even if the monolith architecture is chosen. Your focus should be on how the system is
designed, with implementation being secondary.


**Programming Language used: C# and SQL**


## 3.0 Solution to Task 2.1 - [Rotate Array](https://leetcode.com/problems/rotate-array/description/)

In summary, the task stated that an array of length 'n' will be provided as input along with a value 'k' that denotes the k-th last position of the array. The array needs to be rotated i.e. all the values starting from k to n will be shifted to the start of the array. Furthermore, the array needs to be updated in place. From the following example:

```
Input: nums = [1,2,3,4,5,6,7], k = 3

Output: [5,6,7,1,2,3,4]
```
I could see that the position of the value '1' and '5' got flipped 180 degrees.'5' is located in the kth index of the array. So, to flip it, we need to reverse each part of array individually as follows:

```
Input: nums = [1,2,3,4,5,6,7], k = 3
Expected Output: [5,6,7,1,2,3,4] => [5,6,7] [1,2,3,4]


Output 1: [1,2,3,4] [5,6,7]
Output 2: [4,3,2,1] [7,6,5] => [4,3,2,1,7,6,5] => Reverse each part once
Output 3: [5,6,7,1,2,3,4] => Reversed whole again
```

This reverse can be done in place by having two pointers (one at start and one at end) moving in opposite direction until they intersect or cross each other. The time complexity is O(logn).

## 4.0 Solution to Task 2.2 - [Rising Temperature](https://leetcode.com/problems/rising-temperature/description/)

In summary, we are given a table of temperature on each day. The days are sorted in ascending order. We need to find all days on which the temperature was higher than the previous day. **However, the data contains temperature of some days. So, we only need to consider the temperature that was higher than its previous day (1 day before).**

The main task was to traverse through each row and check if the precding row contains the data of the previous day. If we find such row, we need to subtract the temperature of previous day and see if it is higher. 

I knew that I needed something like array[index-1] in SQL. So, I had to search for it in Google and found that **lag(temperature) ovber (order by recordDate)** clause orders the data by record date to set it in ascending order and then fetches the temperature of the smaller value i.e the previous day.

Using 'with' clause, we can chain the output of multiple SQL query. So, I wrote one query inside the 'with' to fetch record of each row along with the temperature and record date of its previous row. I passed the output through a simple 'where' clause to fetch the id of the rows that have temperature of previous day and it is lower than the current record.

## 5.0 Solution to Task 2.2 - [Employee Bonus](https://leetcode.com/problems/employee-bonus/description/)

This is a simple enough task. We are given two tables - one containing employee information and the other containing the information of bonus of the employees. We need to find the employees that got a bonus less than 1000. An employee not getting a bonus i.e. no rows exist for the employee in bonus table is also considered in the output.

A simple left join was sufficient to fetch the employee information as a left join will also consider missing rows in the 'right' table when concatenating the rows.

## 6.0 Design an Order and Product Management System (.Net)

**Objective:** The goal is to design and implement a system that manages products and
orders using CQRS and event-driven architecture. The system should be loosely coupled
even if the monolith architecture is chosen. Your focus should be on how the system is
designed, with implementation being secondary.


## 6.1 Problem statement

As per my understanding the task has the following requirements:

**Functional requirements**

1. The system will manage products and orders.

2. It should implement a CQRS pattern to separate the commands and queries.

3. The primary objective is to keep the system loosely couples. It can either be a monolith or an event-driven system communicating through RabbitMQ.

4. The primary focus should be the architecture of the design.

5. The secondary objective is to prioritize the implementation details itself. This indicates that the task leans more towards system architecture than development itself.

6. Since the interview is for the position of .Net, we need to use .Net applications.

7. Write unit tests, and basic integration test if possible.


**Non functional requirements**

Scalabale, performant, readable, and testing practices

## 6.2 Design decisions

The application intends to show an approach towards the implementation of an event-driven system. Due to limited time I could not implement the complete system. However, I have attemapted to create an infrastructure that can be used as the building blocks to developing a complete microservice system.

I will discuss the design decisions and architecture here. I will divide the discussion to three different parts:

1. The complete plan that I had in mind

2. The amount of task I was able to complete.

3. How it can be improvized to complete the application.

### 6.2.1 The Complete Plan

CQRS is not just about splitting the commands and queries into multiple services or database. I believe that CQRS is a way to represent a single piece of information in multiple ways to retrieve and add data most efficiently.

For example, let us consider having two tables - Employees and departments with N:1 relationship. Suppose we need to fetch two type of information:

1. All departments as an array of object where each department contains a nestd list of all their employees.

2. All departments that have an employee whose name starts with John.

We can easily see that in the first case, we need a nested list of object which is best suited for a NoSQL database a they deal with objects. 

However, in the second case if we store our data in a NoSQL database, we need to traverse each employee and their corresponding list of employees to find all departments that have employee with a specific name. The time to execute the query will increase as the number of employees and departments increase. In this kind of scenario we need a relational database like SQL to deal with relational data.

Therefore, if both these queries have a very high cost and keeping data of both those queries in a single database (either SQL or NoSQL) will increase the cost of the other type of query. 
Therefore, this is the situation where CQRS fits perfectly. We can create two databases (a SQL and a NoSQL) with two services attached to them. We can have a service that creates employees and saves them in database. After the write serice saves the data, it emits an event. Both the read services subscribe to the event. When they receive the event they store the same piece of information in their own way as to meet their business logic.

Therefore, from my perspective CQRS is a way to consider the technical and business requirements and plan what services and databases an application should have and how the data should flow among them. 

It is not neccessary that each domain should have its own read and write services. Rather, it is about planning how information needs to be represented in the application to have the most efficient read and write performance. If the cost of maintaining two services is significantly higher than the cost of processing data within a single service, then CQRS may do more harm than benefit.

**Therefore, my plan was to demonstrate one of the approaches that we can follow to optimize an application based on its existing architecture and the changing business needs.**

Over the years, I have found that the best starting point is always a monolithic application. Once we have a POC, we can do an alpha or beta test and pinpoint the set of queries that significantly affects the performance of the existing application. Next we decide the best technology that we can use to store and process data related to those queries in order to optimize the performance. Finally, we create a new services and/or  databases based on our analysis.

Therefore, due to shortage of time and to demonstrate the above fact, I have found it best to create a monolithic .Net 6 MVC Web Application first and split the order and product related writes to a different service initially and then split the reads as well on a need basis.

### 6.2.2 Tasks completed

1. **SeliseOMS:** Building a web application where user can create and manage products and orders. We have three tables:

- Product: Contains information related to available products.
- Order: Contains information related to orders placed
- OrderLine: Contains information related to the products purchased in each order.

2. **OrderDB:** Ideally the application should have 4 databases - 2 reads and 2 writes (one for product and the other for order). But to keep it simple, I have created two databases and the reads and writes are performed by different application but on the same database. This violates the principle of CQRS as the database load does not decrease even though the request load in each read/write service decreases. But having database as a bottleneck limits the performance of the system to the performance of the database. With enough time I would have split the databases as well.

3. **MessageBroker:** This is a class library that contains configuration related to RabbitMQ. The message broker will be implemented by most services for communication. At the core, a message broker has two tasks - Consume and Produce. The method to consume and produce events to RabbitMQ is common for all - serializing and deserializing JSON data, and routing them to the appropriate queues via appropriate channels. To follow the DRY principle I have kept these common configuration in a separate class library so that it can be managed as Nuget packages in a Nuget package management repository.

4. **OrderService:** This is a .Net console application that consumes events related to orders, parses them based on event type, and updates the order database. To keep the number of queues at a minimum, I have planned to assign one queue for each service (similar to MSMQ) and send multiple events to the queue with an additional information of 'Event type'. Since the number of events are few, we can receive multiple type of events in the same queue as long as it is related to order. But as the number of event categories and consumer categories increase, we may need to assign new queues for a set of one or more event categories. It will be a logical separation of the queues rather than separation based on the table. Currently this service creates an order and updates the status of an order.

### 6.2.3 Features not implemented

1. **Implementation of product service:** A product service that performs write operation of the product table. It will be a consumer and listen to a queue for events related to the product database. When a user creates an order, the web app will trigger a command to create an order, the order service will receive that and update the database. Finally, it will trigger an event related to 'Order Created' and multicast it to the product service (and other relevant queues) which will then decrease the quantity of available products based on the order line. The product service will also be responsible for creating and updating new products.

2. **Unit tests:** I had plans to write unit tests for atleast the write functionalities of order using XUnit. I would have created a separate project for unit tests related to OrderService.  But I was unable to implement that due to time constraints.

3. **Replacing the databases with event logs:** An event is information related to the changes that has already been made to the system. It usually will not be reverted. If we store the log of each event in a database instead of storing the data itself, we can theoretically replay all the events from start to the end to recreate a database. Practically it sounds very expensive but there are different techniques like compressing a set of past event logs to one. In this way we can simplify synchronizing multiple databases by keeping a log of all events.

4. **Creating more abstractions:** Due to time constraints I had to write a lot of configuration as magic strings which is never desirable. For example, specifying the event catgories and triggering appropriate order handler in the order service based on the event type. I would have updated those and made them completely dynamic using System.Reflection and other relevant packages.



