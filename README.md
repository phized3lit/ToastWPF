# ToastWPF

토스트 메시지 Toast Message
(C# / .Net6.0 / WPF)

안드로이드와 유사한 토스트 메시지를 띄웁니다.
Show a toast message like Android.

<img width="50%" src="https://user-images.githubusercontent.com/60687214/192128310-552a2f81-372c-41aa-bcef-9836651c8b21.gif"/>

<img width="50%" src="https://user-images.githubusercontent.com/60687214/192128311-bb4f0e77-5570-4c51-859d-0c842780760f.gif"/>

Show message</br>
`Toast.Show("Message to show");`

Show message with set time
(Millisecond)</br>
`Toast.Show("Message to show", 1500);`

Set position
(Applied from the next message.)</br>
`Toast.SetPosition(owner: this, horizontalPos: 0.5, verticalPos: 0.8);`
