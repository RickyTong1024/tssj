#!/bin/bash

#make
mv configure.ac configure.in
aclocal
autoconf
autoheader
automake -a
./configure CXXFLAGS="-g -O -std=c++11"
make
mkdir out
./cp.sh
