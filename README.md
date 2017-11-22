# Description
This tool is used to register existing SAP Business One server into a SAP Business One SLD.
It is meant to be used in the automatic creation of SAP Business One environment - once the environment is created, you can use this tool to register it to an existing SAP Business One SLD automatically. You can then log into SAP Business One Client without the need for additional manual work.

# Limitations
* The tool registering to the SLD only support username/password SQL connection. It cannot use Windows authentication or other kind of authentication.


# Requirements
* For the Microsoft Windows platform only.
* Microsoft .Net 4.5.1


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

You can download the tool in the [GitHub release section](../../releases).
Once the latest release has been downloaded, simply unzip the archive and run RegisterDBServerToSLD.exe from a Windows console.


# How to obtain support
This project allows and expects users to post questions or bug reports in the [GitHub bug tracking system](../../issues).

# Contributing
If you would like to contribute, please fork this project and post pull requests.

# License
Copyright (c) 2017 SAP SE or an SAP affiliate company. All rights reserved.
This file is licensed under the Apache Software License, v. 2 except as noted otherwise in the [LICENSE](LICENSE.txt) file.

