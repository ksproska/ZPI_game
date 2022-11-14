using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maps
{
    public class Map
    {
        public int? MapId { get; set; }
        public int? CreatorId { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? CreationDate { get; set; }
        public List<Point> Points { get; set; }
        public Map(List<Point> points, int? mapId = null, int? creatorId = null, DateTime? creationDate = null)
        {
            Points = points;
            MapId = mapId;
            CreatorId = creatorId;
            CreationDate = creationDate;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        private class DateTimeJsonConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options) =>
                    DateTime.ParseExact(reader.GetString()!,
                        "yyyy-MM-dd", CultureInfo.InvariantCulture);

            public override void Write(
                Utf8JsonWriter writer,
                DateTime dateTimeValue,
                JsonSerializerOptions options) =>
                    writer.WriteStringValue(dateTimeValue.ToString(
                        "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}