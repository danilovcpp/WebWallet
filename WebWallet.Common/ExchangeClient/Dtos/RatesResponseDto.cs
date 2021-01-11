﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallet.Common.ExchangeClient.Dtos
{
	public class RatesResponseDto
	{
		public string Base { get; set; }
		public DateTime Date { get; set; }
		public Dictionary<string, decimal> Rates { get; set; }
	}
}
