# SPS

## Grid
The grid is an easy way to construct areas of responsibility in a distributed fashion. Neighboring grains
are also easily distinguished.


# Fault tolerance
When a SPS providing grain is revived as an old version state changes can get lost.

## Heartbeat
An easy way to check if either a client or an provider is out of sync is to send sporadic heartbeats.
The grain that is out of sync can then re-add itself to the SPS network. This basically reconstructs
the lost state.
=> will fail dramatically in case of bigger crashes?   

## Redundancy
Another option might be data redundancies to store the deltas of grain versions in an external distributed
key value store.