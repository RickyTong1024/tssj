#!/bin/bash

#make
autoconf
automake
./configure CXXFLAGS="-g -O0"
make

