import java.util.Scanner;

public class GasMileage{
    public static void main(String[] args) {

        /**
         * 30mi 3
         * 40mi 2
         * 35mi 2
         * 100mi 6
         * 
         * loop through
         * trip 1: x miles per gallon
         * trip 2: y miles per gallon
         * ...
         * 
         * total stats: xyz miles per gallon
         * 
         */

        Scanner trips = new Scanner(System.in);
        int x = 0;
        int y = 0;
        int counter = 0;
        int miles = 0;
        int gallons =0;
        while ((miles != -1) && (gallons != -1)){
        System.out.println("Enter Miles driven or -1 to quit: ");
        miles = trips.nextInt();
        System.out.println("Enter gallons used or -1 to quit: ");
        gallons = trips.nextInt();
        //double mpg = miles / gallons;
        //System.out.println("Trip " + counter + " MPG: " + mpg);

        //while ((miles != -1) && (gallons != -1)){
            x += miles;
            y += gallons;
            counter++;
        double mpg = miles / gallons;
        System.out.println("Trip " + counter + " MPG: " + mpg);
        System.out.println("Total Trip Miles: " + x + " Total Gallons: " + gallons + " Total MPG: " + mpg);
    }
    }
}