# Description
Windows CLI tool to register a SAP Business One server into an existing SAP Business One SLD.

# Limitations
* The tool registering to the SLD only support username/password SQL connection


# Requirements
For the Microsoft Windows platform only.


# Configuration

## Usage 
`RegisterDBServerToSLD /sldserver [SLDSERVER] /sldusername [SLDUSERNAME /sldpassword [SLDPASSWORD] /instancename [INSTANCETOADD] /sqlversion [SQLVERSION] /dbusername [INSTANCETOADDUSERNAME] /dbpassword [INSTANCETOADDPASSWORD]`


## Arguments

* /sldserver - example: localhost if SLD is https://localhost:40000/ControlCenter
* /sldusername - example: B1SiteUser
* /slddassword - Password to the slddbusername
* /instancename - example: b1server,65170 for a named instance
* /sqlversion - The vesion of SQL to use. Values: 
  * HANA
  * SQL2008
  * SQL2008R2
  * SQL2012
  * SQL2014
  * SQL2016
* /dbusername - example: sa 
* /dbpassword - Password to the dbusername


# Download and Installation

## Download
The source code can be cloned and compiled from this GitHub page.

## Installation
The tool does not require to be installed.


# How to obtain support
This project allows and expects users to post questions or bug reports in the GitHub bug tracking system.

# Contributing
If you would like to contribute, please fork this project and post pull requests.

# License
Copyright (c) 2017 SAP SE or an SAP affiliate company. All rights reserved.
This file is licensed under the Apache Software License, v. 2 except as noted otherwise in the [LICENSE](LICENSE.txt) file.

