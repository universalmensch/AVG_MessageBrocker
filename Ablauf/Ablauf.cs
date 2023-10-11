
public static void Main(){
    

    
}

public static ConnectionFactory getConnectionFactory(){
    var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
}

public static void declareQueue(Model channel){
    channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
}

