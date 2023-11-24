## Run the producer application

_All of the following commands are tested in Windows PowerShell, you may need to change a little to run them in Linux._

### Without Docker

If you have .NET 6 SDK installed, you can run this sample dirrectly without docker.

#### Config Kafka client configuration

* Update the `appsettings.json` file with the information from your Confluent environment
* We can use `dotnet user-secrets` tools for credentials.

#### Run the application

```
dotnet run --project .\ConfluentProtobufDotnetExample\
```

There will be 10 messages produced to the configured topic.

### With Docker

You can run this sample without .NET SDK via Docker

#### Build the Docker image

```
docker build . -f .\ConfluentProtobufDotnetExample\Dockerfile -t sample:local
```

#### Run the built Docker image

We can override the .NET configuration by providing environment variable. For example

```
docker run --rm -e "Producer__SaslUsername=<SASL_USERNAME>" -e "Producer__SaslPassword=<SASL_PASSWORD>" -e "SchemaRegistry__BasicAuthUserInfo=<SCHEMA_REGISTRY_USERNAME>:<SCHEMA_REGISTRY_PASSWORD>" sample:local
```

### Expected output of the producer

Whether you run the producer with or without Docker, the output will be ten messages like this

```text
{
  "CustomerId": 0,
  "EventName": "0",
  "Data": "0 data",
  "CreatedDate": "2023-11-24T02:46:19.920634900Z"
}
{
  "CustomerId": 1,
  "EventName": "1",
  "Data": "1 data",
  "CreatedDate": "2023-11-24T02:46:22.505189100Z"
}
{
  "CustomerId": 2,
  "EventName": "2",
  "Data": "2 data",
  "CreatedDate": "2023-11-24T02:46:22.584740300Z"
}
{
  "CustomerId": 3,
  "EventName": "3",
  "Data": "3 data",
  "CreatedDate": "2023-11-24T02:46:22.666560300Z"
}
{
  "CustomerId": 4,
  "EventName": "4",
  "Data": "4 data",
  "CreatedDate": "2023-11-24T02:46:22.747999600Z"
}
{
  "CustomerId": 5,
  "EventName": "5",
  "Data": "5 data",
  "CreatedDate": "2023-11-24T02:46:22.826907100Z"
}
{
  "CustomerId": 6,
  "EventName": "6",
  "Data": "6 data",
  "CreatedDate": "2023-11-24T02:46:22.903534400Z"
}
{
  "CustomerId": 7,
  "EventName": "7",
  "Data": "7 data",
  "CreatedDate": "2023-11-24T02:46:22.982889900Z"
}
{
  "CustomerId": 8,
  "EventName": "8",
  "Data": "8 data",
  "CreatedDate": "2023-11-24T02:46:23.060401900Z"
}
{
  "CustomerId": 9,
  "EventName": "9",
  "Data": "9 data",
  "CreatedDate": "2023-11-24T02:46:23.139360600Z"
}
```

Those messages are both printed to console and sent to Confluent Cloud (the topic name is in the `appsettings.json` file)

## Run the consumer with `kafka-protobuf-console-consumer`

Create a `consumer.properties` file from the `consumer.properties.sample` file then fill the required data there to configure the consumer tool.

Run on Windows (replace the `--bootstrap-server`, `--property schema.registry.url=`, and `--topic` values by your actual values)
```
docker run -it --rm -v ${PWD}:/home/appuser --entrypoint kafka-protobuf-console-consumer -e "SCHEMA_REGISTRY_LOG4J_OPTS=-Dlog4j.configuration=file:/home/appuser/log4j.properties" confluentinc/cp-schema-registry --bootstrap-server pkc-312o0.ap-southeast-1.aws.confluent.cloud:9092 --property schema.registry.url=https://psrc-zy38d.ap-southeast-1.aws.confluent.cloud --consumer.config consumer.properties --formatter-config consumer.properties --topic sample-topic --timeout-ms 10000 --from-beginning
```

For Linux or Mac user, update the `-v` parameter to mount the current directory to `/home/appuser` when run Docker.

### Expected consumer outcome

The consumer should print all messages in the topics like this

```text
CreateTime:1700730423726        Partition:0     Offset:0        {"CustomerId":0,"EventName":"0","Data":"0 data","CreatedDate":"2023-11-23T09:07:02.388195600Z"}
CreateTime:1700730425516        Partition:0     Offset:1        {"CustomerId":1,"EventName":"1","Data":"1 data","CreatedDate":"2023-11-23T09:07:05.515572300Z"}
CreateTime:1700730425656        Partition:0     Offset:2        {"CustomerId":2,"EventName":"2","Data":"2 data","CreatedDate":"2023-11-23T09:07:05.656146700Z"}
CreateTime:1700730425797        Partition:0     Offset:3        {"CustomerId":3,"EventName":"3","Data":"3 data","CreatedDate":"2023-11-23T09:07:05.797243Z"}
CreateTime:1700730425935        Partition:0     Offset:4        {"CustomerId":4,"EventName":"4","Data":"4 data","CreatedDate":"2023-11-23T09:07:05
```

