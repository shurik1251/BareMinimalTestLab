Feature: HomePage
	In order to find special pupils
	As a regular user
	I want to be told the names of pupils having fixed birth date

@mytag
Scenario: See the special pupil
	Given I have entered the home page
	Then the result should be "First Last" on the screen
