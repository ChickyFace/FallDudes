# FallDudes
Flush Games second case

CharacterController,
Root Motion ,
Blend Tree,
Pooling System,
JSON,
NavMesh,
Shader Graph,


öğrenerek kullanmaya çalıştım.



-Tekrar Yapılması Gerekenler

1-) Joystick kontrolü ile movement olacak
2-) Spiral dönen obstacle'lar içinde beklediğimiz zaman bizi döndürmüyor
3-) Kullanıcı suya düşerse, oyuna yerleştirilmiş olan checkpointlerden geri tekrar oyuna dahil olacak
4-) Win panel - Lose panel yok
5-) Her bir yarışmacı oyun sonunda yarışı kaçıncı bitirdiği gözükmeli
6-) Oyun içi leaderboard
7-) Obstacle'ların üstünden sadece space ile atlayıp geçebiliyoruz bu olmasın fall guys mantığında üstünden atlanabilecek obstacle'lar olabilir ama her obstacle'ın da üstünden atlamayalım 


Player'ın controlü baştan yapıldı, CharacterController kullanılmıyor artık onun yerine  playerRigidbody.velocity set ediliyor.Tüm movement kompozisyonu değiştirildi. Leaderboard mertcan'ın dediği gibi yapıldı.
Hissiyat konusunda da elimden geleni yaptım. Tüm kalan eklemelerde yapıldı.