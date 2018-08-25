# Crystal Reports Ninja with Burst capability using ITextSharp

This project is [Crystal Reports Ninja](https://github.com/rainforestnet/CrystalReportsNinja) that enhanced with Burst reporting capability.

What is Bursting of Reports?
[Find out here](https://www.ibm.com/support/knowledgecenter/en/SSRL5J_1.1.0/com.ibm.swg.ba.cognos.ug_cr_rptstd.10.1.1.doc/t_cr_rptstd_modrep_bursting_reports.html)

This project uses [iTextSharp](https://github.com/itext/itextsharp) to split PDF report that generated 
from [Crystal Reports Ninja](https://github.com/rainforestnet/CrystalReportsNinja) by Group Header #1 (the top most level group). 
Each group will be output a one PDF file, please ensure you set “Next Page after” at Group Footer #1.

In a nutshell, it basically splits PDF file based on bookmark within the file, 
the top level group of Crystal Reports will be the second level bookmark in the PDF file.

