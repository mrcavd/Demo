// CppChallenge.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
using namespace std;


// SequenceCheck finds out integers at desirable index that's divisible by 2/3/5 only
// there's option to store everything up to said index
// or for faster approach, only desired index is output

class SequenceCheck {
private:
	int index;
	std::vector<int> list;
public:
	SequenceCheck::SequenceCheck() {
		//std::cout << "added 1 to the list\n";
		addToList(1);
	}

	void addToList(int number) {
		//std::cout << "Adding " << number << "\n";
		list.push_back(number);
	}

	int GetIndex(int index) { return list[index]; }

	const int GetSize() {
		return list.size();
	}

	// print the entire sequence if chosen to store each value
	void PrintResults() {
		for (int i = 0; i < list.size(); i++) {
			std::cout << list[i] << ", ";
		}
	}

	/* 
	 *
	 *			slower recursion approach
	 *
	 *
	bool checkValidity(int number){
		int remainder = number;

		if (remainder == 2 || remainder == 3 || remainder == 5) {
			return true;
		}
		else if (number % 2 == 0) {
			remainder = number / 2;
			//std::cout << "Div by 2 Remainder: " << remainder <<"\n";
		}
		else if (number % 3 == 0) {
			remainder = number / 3;
			//std::cout << "Div by 3 Remainder: " << remainder << "\n";

		}
		else if (number % 5 == 0) {
			remainder = number / 5;
			//std::cout << "Div by 5 Remainder: " << remainder << "\n";
		}
		else {
			return false;
		}


		checkValidity(remainder);
	}
	*/


	// check if a number can be factored by 2, 3, or 5
	// output the remainder after dividing by 2, 3 or 5 once
	int checkValidityQuick(int number) {
		int remainder = number;

		if (number % 2 == 0) {
			remainder = number / 2;
			//std::cout << "Div by 2 Remainder: " << remainder <<"\n";
		}
		else if (number % 3 == 0) {
			remainder = number / 3;
			//std::cout << "Div by 3 Remainder: " << remainder << "\n";

		}
		else if (number % 5 == 0) {
			remainder = number / 5;
			//std::cout << "Div by 5 Remainder: " << remainder << "\n";
		}
		else {
			return -1;
		}

		return remainder;

	}
};

int main()
{
	// Testing code for Part 2
	SequenceCheck check;

	int number = 2;
	int remainder = 3;
	int index = 1;
	
	while(index != 1500) {
		bool recursive = true;
		remainder = number;
		while (recursive) {
			remainder = check.checkValidityQuick(remainder);
			if (remainder == 1) { 
				// uncomment next line if storing each value up to desire index is required
				//check.addToList(number);
				index++;
				number++;
				recursive = false; 
			}
			else if (remainder == -1) {
				recursive = false;
				number++;
			}
		}
	}

	std::cout << index << ", " << number;
	//std::cout<<check.GetIndex(499);

	/*
	for (int i = 0; i < check.GetSize(); i++) {
		std::cout << check.GetIndex(i) << ", ";
	}
	*/

}
