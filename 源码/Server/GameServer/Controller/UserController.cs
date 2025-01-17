﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;
namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO=new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
             requestCode= RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            Console.WriteLine(data);
            string[] strs = data.Split(',');//根据字符对数据进行分割
            
            Console.WriteLine(strs.Length+"||"+strs[1]);
            User user = userDAO.VerifyUser(client.MysqlConn, strs[0],strs[1]);

            if (user == null)
            {
                //  return Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result res = resultDAO.GetResultByUserId(client.MysqlConn, user.Id);
                client.SetUserData(user, res);
                return string.Format("{0},{1},{2},{3}",((int)ReturnCode.Success).ToString(), user.Username, res.TotalCount, res.WinCount);
            }

        }
        public string Register(string data,Client client,Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0]; string password = strs[1];
            bool detUser = userDAO.GetUserByUsername(client.MysqlConn, username);
            if (detUser)
            {
                userDAO.RegisterUser(client.MysqlConn, username, password);
                return ((int)ReturnCode.Success).ToString();

            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }

        }
    }
}
