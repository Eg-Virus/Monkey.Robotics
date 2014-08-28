﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace Xamarin.Robotics.Mobile.Core.Bluetooth.LE
{
	public class LEStream : Stream
	{
		readonly Task initTask;

		readonly IDevice device;
		IService service;
		ICharacteristic receive;
		ICharacteristic transmit;
		ICharacteristic reset;

		static readonly Guid ServiceId = new Guid ("713D0000-503E-4C75-BA94-3148F18D941E");

		static readonly Guid ReceiveCharId = new Guid ("713D0002-503E-4C75-BA94-3148F18D941E");
		static readonly Guid TransmitCharId = new Guid ("713D0003-503E-4C75-BA94-3148F18D941E");
		static readonly Guid ResetCharId = new Guid ("713D0004-503E-4C75-BA94-3148F18D941E");

		public LEStream (IDevice device)
		{
			this.device = device;
			initTask = InitializeAsync ();
		}

		async Task InitializeAsync ()
		{
			Debug.WriteLine ("LEStream: Looking for service " + ServiceId + "...");
			service = await device.GetServiceAsync (ServiceId);
			Debug.WriteLine ("LEStream: Got service: " + service.ID);

			Debug.WriteLine ("LEStream: Getting characteristics...");
			receive = await service.GetCharacteristicAsync (ReceiveCharId);
			transmit = await service.GetCharacteristicAsync (TransmitCharId);
			reset = await service.GetCharacteristicAsync (ResetCharId);
			Debug.WriteLine ("LEStream: Got characteristics");
		}


		#region implemented abstract members of Stream

		public override int Read (byte[] buffer, int offset, int count)
		{
			var t = ReadAsync (buffer, offset, count, CancellationToken.None);
			t.Wait ();
			return t.Result;
		}

		public override async Task<int> ReadAsync (byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			await initTask;
			throw new NotImplementedException ();
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			initTask.Wait ();
			throw new NotImplementedException ();
		}

		public override void Flush ()
		{
			throw new NotImplementedException ();
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ();
		}
		public override void SetLength (long value)
		{
			throw new NotSupportedException ();
		}
		public override bool CanRead {
			get {
				return true;
			}
		}
		public override bool CanSeek {
			get {
				return false;
			}
		}
		public override bool CanWrite {
			get {
				return true;
			}
		}
		public override long Length {
			get {
				return 0;
			}
		}
		public override long Position {
			get {
				return 0;
			}
			set {
			}
		}
		#endregion
	}
}
