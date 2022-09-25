# ToastMessageWPF


* WPF UserControlLibrary
* C#
* .Net6.0
(C# / .Net6.0 / WPF UserControlLibrary)

안드로이드와 유사한 토스트 메시지를 띄웁니다.</br>
Show a toast message like Android.</br>

<img width="50%" src="https://user-images.githubusercontent.com/60687214/192128310-552a2f81-372c-41aa-bcef-9836651c8b21.gif"/>

<img width="50%" src="https://user-images.githubusercontent.com/60687214/192128311-bb4f0e77-5570-4c51-859d-0c842780760f.gif"/>

'ToastWPF.dll' 참조추가</br>
add 'ToastWPF.dll' reference</br>
`using ToastWPF;`

토스트 띄우기.</br>
Show message</br>
`Toast.Show("Message to show");`

시간 지정하여 띄우기.</br>
Show message with set time
(Millisecond)</br>
`Toast.Show("Message to show", 1500);`

위치 지정</br>
Set position</br>
(다음 메시지 부터 적용 됨.)</br>
(Applied from the next message.)</br>
`Toast.SetPosition(owner: this, horizontalPos: 0.5, verticalPos: 0.8);`

[horizontalPos] : left(0) ~ center(0.5) ~ right(1)</br>
[verticalPos] : top(0) ~ center(0.5) ~ bottom(1)</br>
