# AppQuest_Schatzkarte ![data](https://img.shields.io/badge/Status-development-orange.svg)

##Technologie
XAMARIN Forms 2.0

##Aufgabe
Koordinaten eines Posten auslesen und auf Karte anzeigen. Die Koordinaten werden gepsiechert, damit sie später an den Server gesendet werden können.

## Version
0.1

## Authors 
[Luca Marti](https://github.com/zmartl)  
[Homepage](https://www.luca-marti.ch)  
Software Developer
 
[Elia Perenzin](https://github.com/zperee)  
[Homepage](http://eliaperenzin.ch/)  
Software Developer

## Functions
#[Infrastructure/FileSaver.cs](https://github.com/zperee/AppQuest_Schatzkarte/blob/master/AppQuest_Schatzkarte/AppQuest_Schatzkarte/Infrastructure/FileSaver.cs)

**Create and open the local file**  
> return: Task<string>  
```C#
private async Task<IFile> AssureFileExistsAsync()
```

**Read the content from the local file**
```C#
private async Task<IFile> AssureFileExistsAsync()
```

**Save the pins from the map into the local file**  
> params: IList<T>  
```C#
SaveContentToLocalFileAsync<T>(IList<T> content)
```

## Errors
### Error #1: FileSaver.cs: Property is null or emtpy
This error can occured when the filesystem hasn't found the folder/file. 

### Error #2: Homepage.xaml.cs: No GPS Signal avaiable
Maybe the GPS is disabled in your phone-settings. 
=> Turn on GPS. 
