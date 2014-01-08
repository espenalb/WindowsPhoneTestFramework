Feature: AppStart
	In order to verify test framework
	As a WP8 testedr
	I want to be able to install and start the Zedge application

Scenario: Start the app
	Given my app is uninstalled
	And my app is installed
	And my app is running
	Then I wait 5 seconds
	Then take a picture

