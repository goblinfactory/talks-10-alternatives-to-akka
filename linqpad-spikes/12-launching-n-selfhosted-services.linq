<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load ".\10-self-hosted-webapi.linq"

void Main()
{
  var t1 = Task.Run(()=> SelfHost.Run(@"http://*:5010"));
  var t2 = Task.Run(()=> SelfHost.Run(@"http://*:5020"));
  Task.WaitAll(t1,t2);
  
  // prove the sites are running
}
