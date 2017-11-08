namespace RegisterDBServerToSLD
{
    class DatabaseInstance
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ServerVersion { get; set; }
        public string ServerType { get; set; }
        public bool IsTrustedConnection { get; set; }
        public string Username
        {
            get { return this._UserName; }
            set
            {
                this._UserName = IsTrustedConnection ? string.Empty : value;
            }
        }
        public string Password
        {
            get { return this._Password; }
            set
            {
                this._Password = IsTrustedConnection ? string.Empty : value;
            }
        }

        private string _UserName = string.Empty;
        private string _Password = string.Empty;
    }
}
