using FullSerializer;
using System;

public static class JsonApi {
    private static fsSerializer _serializer = new fsSerializer();

    public static void AddConverter(fsConverter converter) {
        _serializer.AddConverter(converter);
    }

    public static string Serialize<T>(T value) {
        fsData data;
        var fail = _serializer.TrySerialize<T>(value, out data);
        if (fail.Failed) { throw new Exception(fail.FailureReason); }

        return fsJsonPrinter.CompressedJson(data);
    }

    public static T Deserialize<T>(string serializedState) {
        fsFailure fail;

        fsData data;
        fail = fsJsonParser.Parse(serializedState, out data);

        if (fail.Failed) { throw new Exception(fail.FailureReason); }

        T deserialized = default(T);
        fail = _serializer.TryDeserialize<T>(data, ref deserialized);

        if (fail.Failed) { throw new Exception(fail.FailureReason); }

        return deserialized;
    }
}
