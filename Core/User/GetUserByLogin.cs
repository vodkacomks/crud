﻿using Core.Infrastructure.Exceptions;
using Core.Infrastructure.Utils;
using Core.User.Interfaces;
using Core.User.Validators;
using DataAcess.Interfaces;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User
{
    public class GetUserByLogin : PasswordEncryptionToDecrypt, IGetUserByLogin
    {
        IUserRepository _userRepository;
        readonly IEmailValidate _emailValidad;
        private readonly UserValidator _UserValidator;

        public GetUserByLogin(IUserRepository userRepository, IEmailValidate emailValidate)
        {
            _userRepository = userRepository;
            _emailValidad = emailValidate;
            _UserValidator = UserValidator.Validate().LoginValidator(_emailValidad);
        }

        public async Task<Model.User> Execute(Model.User user)
        {
            var UserValidated = _UserValidator.Validate(user);

            if (!UserValidated.IsValid)
            {
                throw new ApiDomainException(UserValidated.Errors);
            }

            user.Password = EncryptPassword(user.Password);

            var userDb = await _userRepository.GetByEmailPasswordAsync(user.Email, user.Password);

            if (userDb == null)
            {
                IList<ValidationFailure> validationList = new List<ValidationFailure>();
                ValidationFailure validation = new ValidationFailure("Login do usuário", "E-mail ou senha inválidos.");
                validationList.Add(validation);

                throw new ApiDomainException(validationList);
            }

            if (!userDb.Status)
            {
                IList<ValidationFailure> validationList = new List<ValidationFailure>();
                ValidationFailure validation = new ValidationFailure("Login do usuário", "Valide o usuario no seu Email.");
                validationList.Add(validation);

                throw new ApiDomainException(validationList);
            }

            return userDb;
        }
    }
}
