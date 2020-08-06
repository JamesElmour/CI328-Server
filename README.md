# CI328 Data Driven Game Server
***Accompanying JavaScript client: https://github.com/MicahRClarke/CI328-Client***

The multiplayer internet game coursework for my final year Internet Game Design module offered exciting new challenges. Through much research they were overcome, resulting in a robust game. Capable of hosting many clients and is easily expandable due to the great lengths taken to ensure the server component’s efficiency. Easily handling up to 99 players with multiple concurrent matches. Custom written in C#.

The starting idea for the game was “Breakout 99”, inspired by the popular Tetris 99 and Infringio Royale games. Resulting in a new and unique, battle royale addition. Taking a similarly everlasting game and transplanting its core gameplay loop onto a new gaming trend. Whilst introducing some new gameplay ideas that allow the merging of ideas to work well.

## ECS Architecture
Entity-Component-System is a data-driven technique for designing game engines, with a key focus on performance. The server also includes an ECS game engine, which reflects the construction of the JavaScript client-side engine. This parity was done, not only to ensure the performance of the server, but to make it easier to communicate between the server and client. The data structure nature of components allows their attributes to be easily converted into data for transmission. Appended to the standard ECS design, was a priority attribute for each of the game’s systems. This is used to dictate how frequently system changes are transmitted to the client. This is easily allowable due to the compartmentalization of systems into discrete classes, which could easily be expanded to accommodate this new attribute. Whereas a more traditional game object approach to design would have made this process much more complicated, as there would be no centralized place to house this new attribute.

## Multi-threaded
Immediately it was decided that a single-threaded application would not be appropriate for the server. Instead, threads were used throughout the entire application. One thread was dedicated for checking the connected TCP clients, and processing incoming data from them. Other threads were created to house each match, process its systems, and user input. This takes advantage of the multiple threads which modern CPUs provide, as numerous matches could be updated concurrently.

## Smart Network Utilization
Transmitting data between client and server is encoded into messages within the packet. This unique format allows the client and server to understand the context of the message and its attached payload with minimal bytes. The first byte, the super opcode, dictates to which system the message is relating to. Second byte, the sub opcode, tells the receiver what exact functionality the message wishes to invoke. The third byte, player byte, states whether the message relates to the player or their target. All bytes after that is data used in the invoked method. Messages are collected in a message queue. After which they are all concatenated into an array of data, then sent to the client. Instead of being individually transmitted. This reduces the CPU workload on the client and limits the overhead due to packet headers.

## Screenshots
![Player one targets their opponent – who has lost a life and has 4 remaining.](https://i.imgur.com/jpEa0zT.png)
![Player one has been hit with a ‘multi-ball’ power-up and must juggle two balls at once.](https://i.imgur.com/J4upadN.png)
