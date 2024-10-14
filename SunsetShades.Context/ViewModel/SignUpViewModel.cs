﻿namespace SunsetShades.Context.ViewModel
{
    public class SignUpViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address {  get; set; }

        public ResponseMessage ResponseMessage { get; set; }
    }
}