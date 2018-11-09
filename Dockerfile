FROM microsoft/dotnet:2.1-sdk
WORKDIR /metaheuristics

COPY *.csproj ./
RUN dotnet restore

ENV METAHEURISTICS_PROBLEM_SRC_FILE=/metaheuristics/data/problemInput.ttp
ENV METAHEURISTICS_ALGORITHM_SRC_FILE=/metaheuristics/data/algInput.txt
ENV METAHEURISTICS_LOG_OUTPUT_FILE=/metaheuristics/data/output.csv

COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/Metaheuristics.dll"]
