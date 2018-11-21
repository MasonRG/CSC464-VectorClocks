# CSC464-VectorClocks
Simple implementation and demonstration of using vector clocks in multiple threads.

Simulation run with three nodes, each operating on a separate thread. The nodes can either do work or send a message to another node. Actions are randomly generated for each node at simulation start. Nodes can also receive a message from a sending node.
These operations (that is: doing work, sending a message, receiving a message) all cause the vector clock of a node to update.

During the simulation, events are outputted as they occur, and after the simulation has finished the vector clock history of each node is provided for evaluation.

The results of two simulations and their respective vector clock history charts are shown below.\
#### Clock History Chart Legend:
![image sim1 output](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/history_legend.png)

## Simulation 1

#### Output:
![image sim1 output](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/output14.png)

#### Clock History:
![image sim1 clock history](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/history1.png)

#### Causal Relationships of B-3:
*Causes shown in blue area\
Effects shown in red area\
Independent otherwise*\
![image sim1 clock history](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/history1_shaded.png)

## Simulation 2

#### Output:
![image sim2 output](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/output24.png)

#### Clock History:
![image sim2 clock causality](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/history2.png)

#### Causal Relationships of B-4:
*Causes shown in blue area\
Effects shown in red area\
Independent otherwise*\
![image sim2 clock causality](https://github.com/MasonRG/CSC464-VectorClocks/blob/master/img/history2_shaded.png)
