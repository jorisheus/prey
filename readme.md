# :fox: Predator - :rabbit: Prey simulation

I have always been fascinated by running simulations in computers. 
Back when I just started programming, I already tried to implement the game of life and
my own versions of it. This is just one of them I salvaged from around 10~15
years back and ported to .net core.

It tries to simulate a predator prey relationship.
It consists of a WPF frontend backed by Maui graphics to visualize a
rudimentary map of predators and prey moving around. These 'actors' can 
hunt, move, reproduce; all based on a set of rules of configuration. 

Also, I am fiddling around with optimizing the configurations of the world
to get a rather stable world. The ConfigurationOptimizer console app
runs simultaneous simulations of different configurations and sees
which run longest with a 'healthy' predator and prey population. 
It tries to combine and alter the best runs in the next iteration to 
find the best configuration possible. 

2022-08-24 - Salvaged this from back in the days; ported it to .net core. 
This is still very much a work in progress. 
Looking at the code I wrote around 10 years back, 
there is a lot of room for improvement. But the general idea is there.

