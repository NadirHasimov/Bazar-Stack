// Tester.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
 using namespace std;
int main()
{
	for (int i = 0; i <= 100; i++) {
		int c = i*i;
		if (c > 100) {
			break;
		}
		cout << endl << c;
		
	}
	int x;
	cin >> x;
    return 0;
}

