using System;
using System.IO;
using System.Xml.Serialization;
using libLightpack;

namespace Lightpack_cmd
{
    class Program
    {
        private static Settings setup;

        public static void Main(string[] args)
        {
            try
            {
                XMLRead();
                DoStuff(args);
            }
            catch (Exception e)
            {
                if (File.Exists("settings.xml") == false) // If settings.xml doesn't exists, creates a new one with default values.
                    XMLWrite();

                else // File the file exists, but something else wrong (exp. corrupt file), creates a new one with default values. 
                {
                    File.Delete("settings.xml");
                    XMLWrite();
                }
            }
            finally // Everything should be fine now, let's try again!
            {
                XMLRead();
                DoStuff(args);
            }

        }

        private static void DoStuff(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "ON":
                        ON();
                        break;
                    case "OFF":
                        OFF();
                        break;
                    case "Profile":
                        if (args[1] != "")
                            SetProfile(args[1]);
                        break;
                }
            }
            catch (Exception e) { }
        }

        private static void ON()
        {
            try
            {
                ApiLightpack api = new ApiLightpack { Host = setup.IP, Port = setup.Port };
                api.Connect();
                api.Lock();
                api.SetStatus(Status.On);
                api.UnLock();
                api.Disconnect();
            }
            catch (Exception e) { }
        }

        private static void OFF()
        {
            try
            {
                ApiLightpack api = new ApiLightpack { Host = setup.IP, Port = setup.Port };
                api.Connect();
                api.Lock();
                api.SetStatus(Status.Off);
                api.UnLock();
                api.Disconnect();
            }
            catch (Exception e) { }
        }

        private static void SetProfile(string profile)
        {
            try
            {
                ApiLightpack api = new ApiLightpack { Host = setup.IP, Port = setup.Port };
                api.Connect();
                Status state = api.GetStatus();
                string currentprofile = api.GetProfile();

                if (currentprofile != profile) // If the new profile is different then the current one, i do a profile change
                {
                    api.Lock();
                    api.SetProfile(profile); // SetProfile turn the device ON, even if it was OFF before

                    if (state == Status.Off) // If the device was OFF before SetProfile, i turn it OFF again
                        api.SetStatus(Status.Off);

                    api.UnLock();
                }
                api.Disconnect();
            }
            catch (Exception e) { }
        }


        private static void XMLWrite()
        {
            using (var filestream = new FileStream("settings.xml", FileMode.Create))
            {
                Settings setup = new Settings() { IP = "127.0.0.1", Port = 3636 };
                new XmlSerializer(typeof(Settings)).Serialize(filestream, setup);
            }
        }

        private static void XMLRead()
        {
            using (var filestream = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                setup = (Settings)xs.Deserialize(filestream);
            }
        }

    }
}
