Aqua 엔진 빌드 사전 설정을 편리하게 해 주는 프로그램

"엔진 빌드 가이드"의 엔진 설정 및 빌드 - 사전 설정 부분에서 설명하는 작업을 자동으로 해 준다.



이 프로그램은 .NET Framework 6.0 기반으로 개발되어 있음
집에서 취미로 만들었던 것을 들고 온 것이라, 회사 VS2019 버전과 안 맞음.
VS2019는 .NET 5.0 이전까지만 지원함.

코드를 유지보수할 일은 별로 없을 것 같지만, 만약 새로 빌드가 필요하다면 
Visual Studio Code를 이용하고, 빌드 명령어는 터미널에서 다음 명령어를 입력하면 됨.
"C:\Program Files\dotnet\dotnet.exe" publish -p:PublishProfile=FolderProfile
아니면 Visual Studio 2022가 사용가능한 환경으로 옮겨서 작업.
(Code에서는 Windows Form 디자이너를 사용할 수 없을 것이다)