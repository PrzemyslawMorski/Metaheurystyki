FROM microsoft/dotnet:2.1-sdk
WORKDIR /metaheuristics

COPY *.csproj ./
RUN dotnet restore

ENV METAHEURISTICS_PROBLEM_SRC_FILE=/metaheuristics/problemInput.ttp
ENV METAHEURISTICS_ALGORITHM_SRC_FILE=/metaheuristics/algInput.txt
ENV METAHEURISTICS_LOG_OUTPUT_FILE=/metaheuristics/output.csv

COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/Metaheuristics.dll"]
