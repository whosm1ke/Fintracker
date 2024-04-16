const currencies = [
        {
            "id": "baa5c10e-4a1e-66d8-67d5-58920a25489a",
            "name": "Lek",
            "symbol": "ALL",
            "code": 8
        },
        {
            "id": "4aabf971-db51-a81f-09b1-a68899f19ac6",
            "name": "Algerian Dinar",
            "symbol": "DZD",
            "code": 12
        },
        {
            "id": "09ddf66d-cb26-d872-8086-5763932d6e45",
            "name": "Argentine peso",
            "symbol": "ARS",
            "code": 32
        },
        {
            "id": "980587ba-45c4-9c9b-f030-ab7a8c2b4e24",
            "name": "Australian dollar",
            "symbol": "AUD",
            "code": 36
        },
        {
            "id": "ae6164a6-c57f-b871-6abe-3f3b020e03f4",
            "name": "Bahamian dollar",
            "symbol": "BSD",
            "code": 44
        },
        {
            "id": "0d6850ea-727c-3298-294e-c3101a73f4bf",
            "name": "Bahraini Dinar",
            "symbol": "BHD",
            "code": 48
        },
        {
            "id": "208cd704-2b7e-95b1-f4ec-f19d66bbe909",
            "name": "Taka",
            "symbol": "BDT",
            "code": 50
        },
        {
            "id": "bc17f063-93e4-c57a-898a-e0e499836bba",
            "name": "Armenian Dram",
            "symbol": "AMD",
            "code": 51
        },
        {
            "id": "61bf4439-1de2-38c7-56cc-1d0a73a90bfd",
            "name": "Barbados dollar",
            "symbol": "BBD",
            "code": 52
        },
        {
            "id": "a99a393d-6091-2452-4cd3-da16e73a18a8",
            "name": "Bermuda dollar",
            "symbol": "BMD",
            "code": 60
        },
        {
            "id": "50cd5338-1dec-2c68-c136-f8ed1b0c0406",
            "name": "Ngultrum",
            "symbol": "BTN",
            "code": 64
        },
        {
            "id": "67746d20-06bf-9cee-a50e-c317dac95c94",
            "name": "Boliviano",
            "symbol": "BOB",
            "code": 68
        },
        {
            "id": "a62aae0d-fe6c-bf27-0bbe-241e93317805",
            "name": "Pula",
            "symbol": "BWP",
            "code": 72
        },
        {
            "id": "d13daf3a-5917-6453-492d-90ec2e52737b",
            "name": "Belizean dollar",
            "symbol": "BZD",
            "code": 84
        },
        {
            "id": "098d6bef-1f0c-d407-ab55-d4ff88bb4cb4",
            "name": "Solomon Islands dollar",
            "symbol": "SBD",
            "code": 90
        },
        {
            "id": "b89ce5c7-3169-9354-84d3-522f8d2536f4",
            "name": "Brunei dollar",
            "symbol": "BND",
            "code": 96
        },
        {
            "id": "69cfa9c9-5638-f955-3464-f102bf43c956",
            "name": "Kyat",
            "symbol": "MMK",
            "code": 104
        },
        {
            "id": "4cc05fbf-50f2-d1f8-d0bd-51610a228158",
            "name": "Burundian franc",
            "symbol": "BIF",
            "code": 108
        },
        {
            "id": "b7671c07-a77f-c3d3-4d93-9c5e9889028f",
            "name": "Riel",
            "symbol": "KHR",
            "code": 116
        },
        {
            "id": "50aba398-9450-b0af-2d1c-1f353618cf5a",
            "name": "Canadian dollar",
            "symbol": "CAD",
            "code": 124
        },
        {
            "id": "fb64f189-0049-a6bc-31a7-c32c267afa29",
            "name": "Cape Verdean Escudo",
            "symbol": "CVE",
            "code": 132
        },
        {
            "id": "cb1e55ae-cc06-736e-cbd1-61fafe80ca00",
            "name": "Cayman Islands dollar",
            "symbol": "KYD",
            "code": 136
        },
        {
            "id": "537431a4-c9a6-f342-d309-b532464655b9",
            "name": "Sri Lankan rupee",
            "symbol": "LKR",
            "code": 144
        },
        {
            "id": "6b946950-d383-bbd9-c4bf-6a70152def96",
            "name": "Chilean Peso",
            "symbol": "CLP",
            "code": 152
        },
        {
            "id": "3524bdda-6f82-fdd2-3dbb-977edab17e13",
            "name": "Renminbi Yuan",
            "symbol": "CNY",
            "code": 156
        },
        {
            "id": "62575b6f-ca28-bc1b-6f93-0ce2cac776f8",
            "name": "Colombian Peso",
            "symbol": "COP",
            "code": 170
        },
        {
            "id": "37eb9eed-f485-10e7-66f6-02a63adbe929",
            "name": "Comorian Franc",
            "symbol": "KMF",
            "code": 174
        },
        {
            "id": "165f30c4-5cfa-471c-05a4-faed4f323ba4",
            "name": "Costa Rican Colon",
            "symbol": "CRC",
            "code": 188
        },
        {
            "id": "e6028f58-d2a2-2ea4-8e61-a9a56d55dce7",
            "name": "Kuna",
            "symbol": "HRK",
            "code": 191
        },
        {
            "id": "f4c9605b-2884-b5c1-f1aa-6d6771cb2ce8",
            "name": "Kuban Peso",
            "symbol": "CUP",
            "code": 192
        },
        {
            "id": "38884745-6f37-20d4-d4f0-cce00270c38a",
            "name": "Czech koruna",
            "symbol": "CZK",
            "code": 203
        },
        {
            "id": "fc9cc26e-765d-e8eb-ac79-611c7a7e9f57",
            "name": "Danish krone",
            "symbol": "DKK",
            "code": 208
        },
        {
            "id": "9f684889-df14-2149-1159-3b84cf363225",
            "name": "Dominican Peso",
            "symbol": "DOP",
            "code": 214
        },
        {
            "id": "2a48a4fa-517c-5634-bf3a-3111a467e5ca",
            "name": "Salvadoran Colon",
            "symbol": "SVC",
            "code": 222
        },
        {
            "id": "49c541c8-0d05-faa0-c9ef-7898d57f1c38",
            "name": "Ethiopian Birr",
            "symbol": "ETB",
            "code": 230
        },
        {
            "id": "cb659170-8fbc-c10c-9838-5dbcafd46f63",
            "name": "Nakfa",
            "symbol": "ERN",
            "code": 232
        },
        {
            "id": "5e2481a6-7f0f-26ef-1e33-7a4875746001",
            "name": "Falkland Islands pound",
            "symbol": "FKP",
            "code": 238
        },
        {
            "id": "0cc9a998-0c1c-ee01-4e26-8544d51c54cf",
            "name": "Fiji dollar",
            "symbol": "FJD",
            "code": 242
        },
        {
            "id": "2397d1af-7793-22a8-bed8-53adf1e4a42c",
            "name": "Djibouti franc",
            "symbol": "DJF",
            "code": 262
        },
        {
            "id": "fcd73315-c364-912e-5b5d-49cc5f66f7b9",
            "name": "Dalasi",
            "symbol": "GMD",
            "code": 270
        },
        {
            "id": "627cc874-5d64-20df-1cfb-7e4b873e1043",
            "name": "Gibraltar Pound",
            "symbol": "GIP",
            "code": 292
        },
        {
            "id": "e26ab070-2b5f-9a3f-014d-d8b1e48cd062",
            "name": "Quetzal",
            "symbol": "GTQ",
            "code": 320
        },
        {
            "id": "7ec0f181-ebf1-7ee4-9748-e28a02be0aab",
            "name": "Guinea Franc",
            "symbol": "GNF",
            "code": 324
        },
        {
            "id": "f723a195-c835-14af-6ebd-78d05185526b",
            "name": "Guyana Dollar",
            "symbol": "GYD",
            "code": 328
        },
        {
            "id": "27d44f77-6d68-89e6-c696-3f7e31950552",
            "name": "Gourde",
            "symbol": "HTG",
            "code": 332
        },
        {
            "id": "74d64281-8b43-57ad-f834-43bc7f9d6ff6",
            "name": "Lempira",
            "symbol": "HNL",
            "code": 340
        },
        {
            "id": "3d9e3b10-23c1-8994-c2c2-f73c83d19e1a",
            "name": "Hong Kong dollar",
            "symbol": "HKD",
            "code": 344
        },
        {
            "id": "0543f219-f440-4c16-9e87-19a6021255d3",
            "name": "Forint",
            "symbol": "HUF",
            "code": 348
        },
        {
            "id": "c6a81514-f052-cd0a-fbd3-4f64e920be60",
            "name": "Icelandic krona",
            "symbol": "ISK",
            "code": 352
        },
        {
            "id": "b1dcc197-cebf-9ca5-73e1-53c9f9af56dc",
            "name": "Indian rupee",
            "symbol": "INR",
            "code": 356
        },
        {
            "id": "71396ef7-94ef-d17a-effe-c97066ed6931",
            "name": "Rupee",
            "symbol": "IDR",
            "code": 360
        },
        {
            "id": "a1ca9ec9-af47-3343-e0e4-9ace2f7db922",
            "name": "Iranian real",
            "symbol": "IRR",
            "code": 364
        },
        {
            "id": "00698e2a-ce78-d565-72a4-66c865dc6b09",
            "name": "Iraqi Dinar",
            "symbol": "IQD",
            "code": 368
        },
        {
            "id": "b990e6b0-d8f2-94ff-164b-00807fc5b1ca",
            "name": "New Israeli Shekel",
            "symbol": "ILS",
            "code": 376
        },
        {
            "id": "ec9efcce-1632-1132-2888-7c1e0d55bfc8",
            "name": "Jamaican dollar",
            "symbol": "JMD",
            "code": 388
        },
        {
            "id": "312c13ef-bde4-e6c0-0566-ca29092cdc2a",
            "name": "Yen",
            "symbol": "JPY",
            "code": 392
        },
        {
            "id": "b276b1ed-a834-e426-347d-c05ec3148b22",
            "name": "Tenge",
            "symbol": "KZT",
            "code": 398
        },
        {
            "id": "9afcd2cf-6000-5858-694f-9b45bf4485a7",
            "name": "Jordanian Dinar",
            "symbol": "JOD",
            "code": 400
        },
        {
            "id": "d95a210e-f6dd-79c4-b164-25b8847c1e12",
            "name": "Kenyan Shilling",
            "symbol": "KES",
            "code": 404
        },
        {
            "id": "c2464c7d-c8da-f954-6a72-bb61db4fb8d5",
            "name": "North Korean Won",
            "symbol": "KPW",
            "code": 408
        },
        {
            "id": "730ce6cd-30ba-020e-8326-79cd7bf73fc6",
            "name": "Won",
            "symbol": "KRW",
            "code": 410
        },
        {
            "id": "fe0ba10c-f74c-7de7-125b-2d52d6d919d0",
            "name": "Kuwaiti Dinar",
            "symbol": "KWD",
            "code": 414
        },
        {
            "id": "e64a4dc1-6615-e479-8e75-2622734eff2c",
            "name": "Catfish",
            "symbol": "KGS",
            "code": 417
        },
        {
            "id": "aea546c6-b88d-62d1-3ed5-764689c862b7",
            "name": "Kirk",
            "symbol": "LAK",
            "code": 418
        },
        {
            "id": "6b6bea20-8eb4-7dc3-4592-55bc73633e6a",
            "name": "Lebanese Pound",
            "symbol": "LBP",
            "code": 422
        },
        {
            "id": "666a91eb-1aae-c38a-3e7f-b857991d79b1",
            "name": "Loti",
            "symbol": "LSL",
            "code": 426
        },
        {
            "id": "9a72f7fd-0825-9644-8994-0c6f9875ca26",
            "name": "Liberian dollar",
            "symbol": "LRD",
            "code": 430
        },
        {
            "id": "fa4df1a6-77fc-b6d7-df0f-43e6b4dcb5c9",
            "name": "Libyan Dinar",
            "symbol": "LYD",
            "code": 434
        },
        {
            "id": "f3bd8cf8-5125-f988-8500-f404da3a7885",
            "name": "Pataka",
            "symbol": "MOP",
            "code": 446
        },
        {
            "id": "00b13491-7491-32bf-8b6f-ed0b6e80edab",
            "name": "Kwacha",
            "symbol": "MWK",
            "code": 454
        },
        {
            "id": "a6f9b4af-7ad1-38de-cbea-3bc3848f97f5",
            "name": "Malaysian Ringgit",
            "symbol": "MYR",
            "code": 458
        },
        {
            "id": "aad67631-4e6e-0b90-bbb4-1a6795a791eb",
            "name": "Rufia",
            "symbol": "MVR",
            "code": 462
        },
        {
            "id": "05a969cb-a9e8-aad3-86f6-fbf65f33ea7d",
            "name": "Mauritian Rupee",
            "symbol": "MUR",
            "code": 480
        },
        {
            "id": "466673a2-3fb7-04da-49c0-49fde4e1270b",
            "name": "Mexican Peso",
            "symbol": "MXN",
            "code": 484
        },
        {
            "id": "6067a617-3791-94df-104c-a8e46073ade4",
            "name": "Tugrik",
            "symbol": "MNT",
            "code": 496
        },
        {
            "id": "6407549a-b141-464f-9d96-ec4e7c4176ec",
            "name": "Moldovan Leu",
            "symbol": "MDL",
            "code": 498
        },
        {
            "id": "150839ef-54dd-0f16-c0d3-87f76ab2fca3",
            "name": "Moroccan Dihram",
            "symbol": "MAD",
            "code": 504
        },
        {
            "id": "28ad951e-bab2-3ff3-c90a-638e0232a243",
            "name": "Omani Real",
            "symbol": "OMR",
            "code": 512
        },
        {
            "id": "f21e02af-8a37-a0bd-7fa8-64cb45b60fe5",
            "name": "Namibian dollar",
            "symbol": "NAD",
            "code": 516
        },
        {
            "id": "4c0708fb-5762-ed4d-a9b9-40cbc69f4b06",
            "name": "Nepalese rupee",
            "symbol": "NPR",
            "code": 524
        },
        {
            "id": "04acccc9-da29-bb33-9164-b0d5c7d65cf8",
            "name": "Netherlands Antilles Guilder",
            "symbol": "ANG",
            "code": 532
        },
        {
            "id": "10ca8f59-a96b-3c72-585a-205136396805",
            "name": "Aruban Florin",
            "symbol": "AWG",
            "code": 533
        },
        {
            "id": "b0226103-7a5b-73be-e47b-0cc16207bfc0",
            "name": "Vatu",
            "symbol": "VUV",
            "code": 548
        },
        {
            "id": "da552dc7-68c5-9406-e96d-4835df415afd",
            "name": "New Zealand Dollar",
            "symbol": "NZD",
            "code": 554
        },
        {
            "id": "a8230319-6bb6-c76d-5465-2c1944d95677",
            "name": "Cordoba Oro",
            "symbol": "NIO",
            "code": 558
        },
        {
            "id": "09773035-9f48-07ff-09de-bbed9a2f1a74",
            "name": "Naira",
            "symbol": "NGN",
            "code": 566
        },
        {
            "id": "103ec386-c5b8-bd7f-3cde-110ac63aeab6",
            "name": "Norwegian Krone",
            "symbol": "NOK",
            "code": 578
        },
        {
            "id": "4e297059-2fa8-d384-43b7-cd3d476a7b86",
            "name": "Pakistani Rupee",
            "symbol": "PKR",
            "code": 586
        },
        {
            "id": "7534c802-3c83-c3b0-7fbd-a60687d306ac",
            "name": "Balboa",
            "symbol": "PAB",
            "code": 590
        },
        {
            "id": "f0e6df3e-10f0-51fc-0c4a-48f5453ace31",
            "name": "Kina",
            "symbol": "PGK",
            "code": 598
        },
        {
            "id": "b0488565-ced2-24d6-7e9e-134936266d4a",
            "name": "Guarani",
            "symbol": "PYG",
            "code": 600
        },
        {
            "id": "aaf725d9-2153-756c-e5e6-37eaf1e914fb",
            "name": "Nuevo Sol",
            "symbol": "PEN",
            "code": 604
        },
        {
            "id": "fdd8de74-9965-12d5-65a3-cc4b8920a84a",
            "name": "Philippine Peso",
            "symbol": "PHP",
            "code": 608
        },
        {
            "id": "508f5661-b69e-c29a-3339-8da1535910ca",
            "name": "Qatari Real",
            "symbol": "QAR",
            "code": 634
        },
        {
            "id": "e1369e36-8db6-9ef3-90f8-51f6d1c1b914",
            "name": "Rwandan franc",
            "symbol": "RWF",
            "code": 646
        },
        {
            "id": "ccfc81a8-6f0a-34b8-44ae-31a331c2c376",
            "name": "Saint Helena pound",
            "symbol": "SHP",
            "code": 654
        },
        {
            "id": "a0e5d67a-28d2-2e1b-863f-b6674fad77e7",
            "name": "Saudi Real",
            "symbol": "SAR",
            "code": 682
        },
        {
            "id": "4dbe660c-89f8-d33c-f7eb-a12fd7432883",
            "name": "Seychelles Rupee",
            "symbol": "SCR",
            "code": 690
        },
        {
            "id": "7bee7921-01ea-45c2-3c74-9d4e48283db2",
            "name": "Leone",
            "symbol": "SLL",
            "code": 694
        },
        {
            "id": "45c1e153-81af-451b-7746-67e10582e287",
            "name": "Singapore dollar",
            "symbol": "SGD",
            "code": 702
        },
        {
            "id": "610fa08a-5c1f-6b5c-da70-b834289e0550",
            "name": "Dong",
            "symbol": "VND",
            "code": 704
        },
        {
            "id": "5920fe98-a665-0b72-0dd8-65985b976714",
            "name": "Somali shilling",
            "symbol": "SOS",
            "code": 706
        },
        {
            "id": "2dfd4476-5b6e-2eb9-22ec-01588c3f29ca",
            "name": "Rand",
            "symbol": "ZAR",
            "code": 710
        },
        {
            "id": "aa21ac5d-cecb-3be6-fc44-3fbb1e6ea9ea",
            "name": "South Sudanese Pound",
            "symbol": "SSP",
            "code": 728
        },
        {
            "id": "cda0e4ea-7b8b-c69e-561d-ec7e85515584",
            "name": "Lilangeni",
            "symbol": "SZL",
            "code": 748
        },
        {
            "id": "081286db-b093-a941-79be-c942ac1e6375",
            "name": "Swedish krona",
            "symbol": "SEK",
            "code": 752
        },
        {
            "id": "45ff3b2a-ee68-d7d3-dca9-2afc72416558",
            "name": "Swiss Franc",
            "symbol": "CHF",
            "code": 756
        },
        {
            "id": "5da4678c-7dcf-4997-266a-a91919fcbd67",
            "name": "Syrian pound",
            "symbol": "SYP",
            "code": 760
        },
        {
            "id": "4f4a6305-ee6f-9d04-f913-cf92b4365cc5",
            "name": "Bath",
            "symbol": "THB",
            "code": 764
        },
        {
            "id": "3edb2b51-36bf-1b1d-416d-6b02ce196449",
            "name": "Panga",
            "symbol": "TOP",
            "code": 776
        },
        {
            "id": "ec5ab8ea-43a0-254a-acee-028932266fa4",
            "name": "Trinidad and Tobago dollar",
            "symbol": "TTD",
            "code": 780
        },
        {
            "id": "24a79f94-addc-da4a-113a-cd0de576722c",
            "name": "UAE Dihram",
            "symbol": "AED",
            "code": 784
        },
        {
            "id": "8eca061a-4d16-4ca2-77ba-d38ad220c8a4",
            "name": "Tunisian Dinar",
            "symbol": "TND",
            "code": 788
        },
        {
            "id": "16964f87-9a90-eb24-3f11-0468821eb898",
            "name": "Ugandan Shilling",
            "symbol": "UGX",
            "code": 800
        },
        {
            "id": "ca228728-08e2-00cc-9703-4d88a8391d7d",
            "name": "Denari",
            "symbol": "MKD",
            "code": 807
        },
        {
            "id": "f1a8074a-fb2a-ea3b-e385-8429d6fe5770",
            "name": "Egyptian pound",
            "symbol": "EGP",
            "code": 818
        },
        {
            "id": "1280ee0e-2ca5-9085-cceb-f0603f248a95",
            "name": "Pound Sterling",
            "symbol": "GBP",
            "code": 826
        },
        {
            "id": "15bd21c5-9874-26ed-d1cd-0506cdb6bc98",
            "name": "Tanzanian Shilling",
            "symbol": "TZS",
            "code": 834
        },
        {
            "id": "c6746fe4-eb4c-1746-0c5e-88d8748deebc",
            "name": "US dollar",
            "symbol": "USD",
            "code": 840
        },
        {
            "id": "afd132d1-6988-220f-fbe5-9c5cd1333029",
            "name": "Uruguayan peso",
            "symbol": "UYU",
            "code": 858
        },
        {
            "id": "b5c43265-576f-2286-3e59-fb97117d71a1",
            "name": "Uzbek Sum",
            "symbol": "UZS",
            "code": 860
        },
        {
            "id": "1c0c4138-af8f-1e12-5bc2-c3ff8231f09a",
            "name": "Tala",
            "symbol": "WST",
            "code": 882
        },
        {
            "id": "27f3fc90-cb56-d905-630c-04a385401875",
            "name": "Yemeni Real",
            "symbol": "YER",
            "code": 886
        },
        {
            "id": "b4acae88-4183-9fd3-3f49-e8f6e6a3f536",
            "name": "New Taiwanese dollar",
            "symbol": "TWD",
            "code": 901
        },
        {
            "id": "7a9079a9-5f68-7e3b-79ee-a6c8239f32a7",
            "name": "Oguya",
            "symbol": "MRU",
            "code": 929
        },
        {
            "id": "da681d16-6d54-51f7-1bf0-1c24a507fdc8",
            "name": "Dobra",
            "symbol": "STN",
            "code": 930
        },
        {
            "id": "9d95080a-0ce5-0916-551e-11d6821c6eda",
            "name": "Convertible Peso",
            "symbol": "CUC",
            "code": 931
        },
        {
            "id": "e00492c7-6b4b-ee35-1360-ff089b2b670a",
            "name": "Zimbabwe dollar",
            "symbol": "ZWL",
            "code": 932
        },
        {
            "id": "ba198772-1ad9-d525-8052-dfcda2ac185c",
            "name": "Tunkmen New Manat",
            "symbol": "TMT",
            "code": 934
        },
        {
            "id": "572b7ef4-da2c-51be-a407-70c14552136d",
            "name": "Ghanaian cedi",
            "symbol": "GHS",
            "code": 936
        },
        {
            "id": "a9c5a1f9-f3ca-67f9-c7e9-87b3340f7250",
            "name": "Bolivar",
            "symbol": "VEF",
            "code": 937
        },
        {
            "id": "d528de2f-53fa-45a3-2288-228ac4ccfb33",
            "name": "Sudanese Pound",
            "symbol": "SDG",
            "code": 938
        },
        {
            "id": "24ea0104-20b4-9961-c2be-ec0ee8103f77",
            "name": "Uruguayan Peso",
            "symbol": "UYI",
            "code": 940
        },
        {
            "id": "aa33d54f-9a46-4d04-2a1c-7f6b736b1862",
            "name": "Serbian Dinar",
            "symbol": "RSD",
            "code": 941
        },
        {
            "id": "07c703c1-bb93-fd4a-9b12-23c4cf4bbee1",
            "name": "Mozambican Montical",
            "symbol": "MZN",
            "code": 943
        },
        {
            "id": "7c4e5fbb-a72d-8a87-9978-8ddb8e251b78",
            "name": "Azerbaijani Manat",
            "symbol": "AZN",
            "code": 944
        },
        {
            "id": "942a2c01-11bd-6897-865c-84213d7f9829",
            "name": "Romanian Leu",
            "symbol": "RON",
            "code": 946
        },
        {
            "id": "23704fad-5321-7051-d931-b5c28d606817",
            "name": "WIR Euro",
            "symbol": "CHE",
            "code": 947
        },
        {
            "id": "f6617071-9bb1-31cd-fb7b-530103321770",
            "name": "WIR Frank",
            "symbol": "CHW",
            "code": 948
        },
        {
            "id": "57e4b424-a3c2-2fee-c7e8-c41c6e97dfac",
            "name": "Turkish Lira",
            "symbol": "TRY",
            "code": 949
        },
        {
            "id": "555b25cc-499f-a0f0-6f75-3071442342fc",
            "name": "West African franc",
            "symbol": "XAF",
            "code": 950
        },
        {
            "id": "0107ca16-0109-1fc8-5a13-733ae1d878a8",
            "name": "East Caribbean dollar",
            "symbol": "XCD",
            "code": 951
        },
        {
            "id": "f3d525fa-1e08-3649-bfdd-250cef110938",
            "name": "French Pacific franc",
            "symbol": "XPF",
            "code": 953
        },
        {
            "id": "db503c9d-8b0f-4148-0150-37a6953ea063",
            "name": "Special drawing rights (SDRs)",
            "symbol": "XDR",
            "code": 960
        },
        {
            "id": "7d31ed39-31b3-8f11-b2b7-1c1209482451",
            "name": "A notional unit of the AfDB",
            "symbol": "XUA",
            "code": 965
        },
        {
            "id": "511741a5-5388-84e9-e67f-aadfceaabe65",
            "name": "Zambian Kwacha",
            "symbol": "ZMW",
            "code": 967
        },
        {
            "id": "550d0b3d-eb23-6af7-e957-b8b962ab8f05",
            "name": "Surinamese dollar",
            "symbol": "SRD",
            "code": 968
        },
        {
            "id": "b658587f-e6c1-0f5b-34ef-c3ab9bf62711",
            "name": "Malagasy Ariari",
            "symbol": "MGA",
            "code": 969
        },
        {
            "id": "0f532169-5ae0-cf31-34b6-58f2e28f8b5f",
            "name": "Unidad de Valor Real",
            "symbol": "COU",
            "code": 970
        },
        {
            "id": "a90304a9-9d36-1896-cb2a-935d6051fbef",
            "name": "Afghani",
            "symbol": "AFN",
            "code": 971
        },
        {
            "id": "4f95d034-f81f-0565-4a5c-2f99d6aea651",
            "name": "Somoni",
            "symbol": "TJS",
            "code": 972
        },
        {
            "id": "91b480f3-c948-aee2-a5f0-9f4b265951f8",
            "name": "Kwanzaa",
            "symbol": "AOA",
            "code": 973
        },
        {
            "id": "f973412a-b71c-3150-3206-583a23fcbdf6",
            "name": "Belarusian Ruble",
            "symbol": "BYR",
            "code": 974
        },
        {
            "id": "03ec79aa-5041-e295-150d-29780357b017",
            "name": "Bulgarian Lion",
            "symbol": "BGN",
            "code": 975
        },
        {
            "id": "f421047e-ca3f-84b4-422a-6b76a441c1c6",
            "name": "Congolese franc",
            "symbol": "CDF",
            "code": 976
        },
        {
            "id": "96a1fc14-3427-128e-647b-3af7dba2c63c",
            "name": "Convertible Mark",
            "symbol": "BAM",
            "code": 977
        },
        {
            "id": "4e48a223-233a-ea80-5ee7-89e1a228181b",
            "name": "Euro",
            "symbol": "EUR",
            "code": 978
        },
        {
            "id": "b4e64c67-90cb-72db-4302-ceb2dbd7c20c",
            "name": "Mexican Unidad de Inversion (UDI)",
            "symbol": "MXV",
            "code": 979
        },
        {
            "id": "ff659ed4-aee0-4a54-cee0-04095560520d",
            "name": "Hryvnia",
            "symbol": "UAH",
            "code": 980
        },
        {
            "id": "73b9b40a-a8b2-9942-d826-cba893bf99a7",
            "name": "Lari",
            "symbol": "GEL",
            "code": 981
        },
        {
            "id": "4f006dc6-48b7-02b4-0f3b-c23d8492d998",
            "name": "Midol",
            "symbol": "BOV",
            "code": 984
        },
        {
            "id": "6d9e1f0b-ea47-a48b-b5d9-061f036a253b",
            "name": "Zloty",
            "symbol": "PLN",
            "code": 985
        },
        {
            "id": "bb841eac-f076-50d6-95d9-902d4c7e8eb8",
            "name": "Brazilian Real",
            "symbol": "BRL",
            "code": 986
        },
        {
            "id": "10b5e0e7-3059-cf59-20c5-669875dd3f24",
            "name": "Unidad de Fomento",
            "symbol": "CLF",
            "code": 990
        }
];

export default currencies;