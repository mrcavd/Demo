// SGI_demo.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;


class Point2D {
	float x, y;
public:
	Point2D(float setX, float setY) {
		x = setX;
		y = setY;
	}

	float* getCoordinate();

	void printPoint() {
		std::cout << "Point (" << x << ", " << y << ") ";
	}
};

float* Point2D::getCoordinate()
{
	static float coordinate[2];

	coordinate[0] = Point2D::x;
	coordinate[1] = Point2D::y;

	return coordinate;
}

// assume all Rect extends toward right & up from origin by width and length
class Rect2D {
	float width;
	float length;
	float origin[2] = { 0, 0 };

public:

	Rect2D(float x, float y, Point2D* p) {
		width = x;
		length = y;
		setOrigin(p);
	}

	Rect2D(const Rect2D &rectRef) {
		width = rectRef.width;
		length = rectRef.length;
		origin[0] = rectRef.origin[0];
		origin[1] = rectRef.origin[1];
		std::cout << "Rect2D copied\n";
	}

	Rect2D& operator=(const Rect2D& rectRef) {
		if (this != &rectRef) {
			width = rectRef.width;
			length = rectRef.length;
			origin[0] = rectRef.origin[0];
			origin[1] = rectRef.origin[1];
		}
		cout << "Rect2D assigned\n";
		return *this;
	}

	void setOrigin(Point2D* p) {
 		origin[0] = p->getCoordinate()[0];
		origin[1] = p->getCoordinate()[1];
		//std::cout << "Set origin to (" << origin[0] << ", " << origin[1] << ")\n";
	}

	Point2D getOrigin() {
		return Point2D(origin[0], origin[1]);
	}

	bool checkOverlapPoint(Point2D* p) {
		
		std::cout << "Rect width: " << origin[0] << " - " << origin[0] + width << "\n";
		std::cout << "Rect length: " << origin[1] << " - " << origin[1] + length << "\n";

		float pointX = p->getCoordinate()[0];
		float pointY = p->getCoordinate()[1];

		//std::cout << "Point (" << pointX << ", " << pointY << ").\n";
		p->printPoint();
		// if point's x coordinate is within range of origin to width of the rect
		if (pointX <= origin[0] + width &&
			pointX >= origin[0]) {
			
			//std::cout << "within X range.\n";
			// if point's y is within origin to height of the rect
			if (pointY <= origin[1] + length &&
				pointY >= origin[1]) {

				//std::cout << "within Y range.\n";
				
				std::cout << "is contained in rect.\n";
				return true;
			}
		}
		std::cout << "is not contained in rect.\n";
		return false;
	}

	bool checkOverlapRect(Rect2D* rect) {
		float x_diff = abs(rect->origin[0] - this->origin[0]);
		float y_diff = abs(rect->origin[1] - this->origin[1]);

		//std::cout << x_diff << ", " << y_diff << "\n";

		if (x_diff <= rect->width || x_diff <= this->width) {
			if (y_diff <= rect->length || y_diff <= this->length) {
				std::cout << "the two rects intersect.\n";
				return true;
			}
		}
		std::cout << "the two rects don't intersect.\n";
		return false;
	}
};

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

	const int GetSize() {
		return list.size();
	}

	/*
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
		if (number == 1) { return number; }
		if (number % 2 == 0) {
			remainder = number / 2;
			//std::cout << number << " Div by 5 Remainder: " << remainder <<"\n";
		}
		else if (number % 3 == 0) {
			remainder = number / 3;
			//std::cout << number << " Div by 3 Remainder: " << remainder << "\n";

		}
		else if (number % 5 == 0) {
			remainder = number / 5;
			//std::cout << number << " Div by 2 Remainder: " << remainder << "\n";
		}
		else {
			return -1;
		}

		return remainder;

	}

	// check combinations of (2^a)(3^b)(5^c) returns the next smallest non duplicate number
	// interations is the seeking index of the sequence
	int checkValidityQuickQuick(int interations) {
		int count = 1;
		int a = 0, b = 0, c = 0;
		int result;

		do {
			// the algorithm is found here https://math.stackexchange.com/questions/197746/get-numbers-that-have-only-2-3-and-5-as-prime-factors with Bill Dubuque's answer
			// the idea is starting a sequence with 1 where 2 / 3 / 5 are assigned to index a = b = c = 0
			// increment the assigned index of a, b, c by 1 depending on whether 2 * index[a], 3 * index[b] , 5 * index[c] returns the 
			// smallest non duplicate result.
			result = smallest(2 * list[a], 3 * list[b], 5 * list[c]);
			list.push_back(result);

			count++;
			
			if (result == 2 * list[a]) {
				a++;
			}

			if (result == 3 * list[b]) {
				b++;
			}
			
			if (result == 5 * list[c]) {
				c++;
			}
		} while (count < interations);

		return result;
	}

	int smallest(int x, int y, int z) {
		return std::min(std::min(x, y), z);
	}
};

int main()
{
	// Testing code for Part 1

	Point2D p1(2.2f, 3.3f);
	Point2D p2(8, 7);
	Point2D p3(0, 0);

	Rect2D r1(5.8f, 4, &p1);
	Rect2D r2(4, 4, &p2);
	Rect2D r3(3, 3, &p3);

	
	
	cout << "Check if r1 overlaps p1\n";
	r1.checkOverlapPoint(&p1);

	cout << "Check if r1 overlaps p2\n";
	r1.checkOverlapPoint(&p2);

	cout << "Check if r1 overlaps r2\n";
	r1.checkOverlapRect(&r2);

	cout << "Check if r2 overlaps r3\n";
	r2.checkOverlapRect(&r3);

	cout << "r4 is a copy of r3\n";
	Rect2D r4(r3);
	cout << "Check if r2 overlaps r4\n";
	r2.checkOverlapRect(&r4);

	cout << "Check if r2 overlaps r1\n";
	r2.checkOverlapRect(&r1);

	cout << "r4 is assigned with r2\n";
	Rect2D& tempRect = r2;
	r4 = tempRect;
	cout << "Check if r4 overlaps r2\n";
	r4.checkOverlapRect(&r2);

	r1.setOrigin(&p3);

	

	// Testing code for Part 2
	
	SequenceCheck check;

	int index = 1500;
	int number = -1;
	number = check.checkValidityQuickQuick(index);
	std::cout << "\n" << index << " index is: " << number << "\n";

	/*
	int number = 1;
	int remainder = 3;
	int index = 1;
	
	while(index < 1500) {
		bool recursive = true;
		remainder = number;
		while (recursive) {
			remainder = check.checkValidityQuick(remainder);
			// 
			if (remainder == 1) { 
				// uncomment next line if storing each value up to desire index is required
				//check.addToList(number);
				//std::cout << index << ", " << number << "\n";
				index++;
				number++;
				recursive = false; 
				
			}
			// remainder == -1 meaning the number can't be factored by 2 / 3 / 5
			else if (remainder == -1) {
				recursive = false;
				number++;
			}
		}
	}
	*/

	/*
	for (int i = 0; i < check.GetSize(); i++) {
		std::cout << check.GetIndex(i) << ", ";
	}
	*/

}
