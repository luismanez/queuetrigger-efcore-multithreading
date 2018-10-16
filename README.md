# queuetrigger-efcore-multithreading
Project shows how easy is to get issues when using Entity Framework Core in an Azure QueueTrigger Function, as the function can have multiple instances running in parallel, and DbContext is not Thread-safe.
It´s used to ask for help in StackOverflow and find a good approach to solve the problem. I will update the project if I find a good solution...

Here is the question posted in StackOverflow
[https://stackoverflow.com/questions/52830562/issue-insert-update-ef-core-dbcontext-in-azure-queuetrigger-function-multi-thre](https://stackoverflow.com/questions/52830562/issue-insert-update-ef-core-dbcontext-in-azure-queuetrigger-function-multi-thre)
