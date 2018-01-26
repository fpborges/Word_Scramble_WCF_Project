using System.Runtime.Serialization;

namespace FPBWordScramble
{
    class WordScrambleExceptions
    {
        [DataContract]
        public class GameBeingHostedException
        {
            [DataMember]
            public string HostMessage { get; set; }
        }
        [DataContract]
        public class MaximumNumberOfPlayersReachedException
        {
            [DataMember]
            public string MaxPlayersMessage { get; set; }
        }
        [DataContract]
        public class HostCannotJoinTheGameException
        {
            [DataMember]
            public string HostJoinMessage { get; set; }
        }
        [DataContract]
        public class GameIsNotBeingHostedException
        {
            [DataMember]
            public string NonHostMessage { get; set; }
        }
        [DataContract]
        public class PlayerNotPlayingTheGameException
        {
            [DataMember]
            public string PlayerNotPlaying { get; set; }
        }
    }
}