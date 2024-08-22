setlocal

@rem enter this directory
cd /d %~dp0

set TOOLS_PATH=..\ProtoTools

if not exist "./AutoGenerated" mkdir AutoGenerated

%TOOLS_PATH%\protoc.exe Protos/Greet.proto -I ./Protos --csharp_out ./AutoGenerated --grpc_out ./AutoGenerated --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe
%TOOLS_PATH%\protoc.exe Protos/Greet.proto -I ./Protos  --csharp_out ./AutoGenerated

%TOOLS_PATH%\protoc.exe Protos/RtpService.proto -I ./Protos --csharp_out ./AutoGenerated --grpc_out ./AutoGenerated --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe
%TOOLS_PATH%\protoc.exe Protos/RtpService.proto -I ./Protos  --csharp_out ./AutoGenerated

:%TOOLS_PATH%\protoc.exe Protos/cmmData.proto -I ./Protos  --csharp_out ./AutoGenerated

:%TOOLS_PATH%\protoc.exe Protos/cmmService.proto   -I ./Protos  --csharp_out ./AutoGenerated   
:%TOOLS_PATH%\protoc.exe Protos/cmmService.proto   -I ./Protos  --grpc_out ./AutoGenerated --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

:%TOOLS_PATH%\protoc.exe Protos/cmmServiceReciever.proto   -I ./Protos  --csharp_out ./AutoGenerated   
:%TOOLS_PATH%\protoc.exe Protos/cmmServiceReciever.proto   -I ./Protos  --grpc_out ./AutoGenerated --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

:%TOOLS_PATH%\protoc.exe -I Protos --csharp_out ./AutoGenerated Protos/cmmServices.proto  --grpc_out ./AutoGenerated --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe


endlocal